using Smart_Electric_Metering_System_BackEnd.Entities;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Repositories;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Services;
using Smart_Metering_System_BackEnd.Models.Enums;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Smart_Electric_Metering_System_BackEnd.Implementations.Services;

public class DataService : IDataService
{
    IMeterRepo _meterRepo;
    IMeterUnitsRepo _meterUnitsRepo;
    IMeterUnitAllocationRepo _meterUnitAllocationRepo;
    IMeterPromptService _meterPromptService;
    IPricesRepo _pricesRepo;
    ICustomerService _cutomerService;
    IMeterService _meterService;
    IMeterUnitAllocationService _meterUnitAllocationService;
    public DataService(IMeterRepo meterRepo, IMeterUnitsRepo meterUnitsRepo, IMeterUnitAllocationRepo meterUnitAllocationRepo, IMeterPromptService meterPromptService, IPricesRepo pricesRepo, ICustomerService customerService, IMeterService meterService, IMeterUnitAllocationService meterUnitAllocationService)
    {
        _meterService = meterService;
        _meterUnitAllocationService = meterUnitAllocationService;
        _cutomerService = customerService;
        _meterRepo = meterRepo;
        _meterUnitsRepo = meterUnitsRepo;
        _meterUnitAllocationRepo = meterUnitAllocationRepo;
        _meterPromptService = meterPromptService;
        _pricesRepo = pricesRepo;
    }
    public async Task<ESP32Response> EstablishConnection(string MeterId, string auth)
    {
        var meter = await _meterRepo.Get(x => x.MeterId == MeterId && x.ConnectionAuth == auth);
        if (meter != null)
        {
            meter.IsActive = true;
            await _meterRepo.Update(meter);
            return new ESP32Response
            {
                IsActive = meter.IsActive,
                ActiveLoad = meter.ActiveLoad,
                TotalUnits = meter.TotalUnits,
                ConsumedUnits = meter.ConsumedUnits,
                Status = true
            };
        }
        return new ESP32Response
        {
            Status = false,
            Message = "Unable to establish Connection. Invalid Authentication Details!"
        };
    }
    public async Task<BaseResponse> MeterUnitsDataFromESP32(CreateMeterUnitsDto createMeterUnitsDto){
        var meter = await _meterRepo.Get(x => x.MeterId == createMeterUnitsDto.MeterId && x.ConnectionAuth == createMeterUnitsDto.ConnectionAuth);
        if(meter != null && meter.IsActive == true && meter.TotalUnits > meter.ConsumedUnits){
            var powerInkWh = createMeterUnitsDto.PowerValue;
            var meterUnitAllocationResolve = await ResolveUnitAllocation(meter.Id, powerInkWh);
            if(meterUnitAllocationResolve.Item1 == true && meterUnitAllocationResolve.Item2 == true && meterUnitAllocationResolve.Item3 == 0){
                meter.ConsumedUnits += powerInkWh;
            }
            else if(meterUnitAllocationResolve.Item1 == false && meterUnitAllocationResolve.Item2 == false && meterUnitAllocationResolve.Item3 > 0){
                meter.ConsumedUnits += powerInkWh - meterUnitAllocationResolve.Item3;
                powerInkWh -= meterUnitAllocationResolve.Item3;
            }
            var unit = new MeterUnits () {
                MeterId = meter.Id,
                PowerValue = createMeterUnitsDto.PowerValue,
                VoltageValue = createMeterUnitsDto.VoltageValue,
                CurrentValue = createMeterUnitsDto.CurrentValue,
                PowerFactorValue = 0.00,
                TimeValue = DateTime.Now,
                ConsumptionValue = powerInkWh,
                ElectricityCost = (await _pricesRepo.Get(x => x.Id == 1)).Rate * powerInkWh,
            };
            if(meterUnitAllocationResolve.Item1 == false && meterUnitAllocationResolve.Item2 == false && meterUnitAllocationResolve.Item3 == 0){
                unit = null;
            }
            if(unit != null){
                await _meterUnitsRepo.Create(unit);
            }
            await _meterRepo.Update(meter);
            if(createMeterUnitsDto.Status == false){
                var meterPrompt = new CreateMeterPromptDto{
                    MeterId = meter.MeterId,
                    ConnectionAuth = meter.ConnectionAuth,
                    Title = "High Voltage Warning",
                    Description = $"The meter {meter.MeterId} recorded voltages above the maximum operating limit. The meter will reconnect momentarily.",
                    Type = MeterPromptType.VoltageOverload
                };
                await _meterPromptService.CreateMeterPrompt(meterPrompt);
            }
            return new BaseResponse{
                Status = true
            };
        }
        else if(meter != null && meter.TotalUnits <= meter.ConsumedUnits){
            meter.ActiveLoad = false;
            await _meterRepo.Update(meter);
            return new BaseResponse{
            Status = false,
            Message = "Meter Units Exceeded. Kindly Buy More Units!"
        };
        }
        return new BaseResponse{
            Status = false,
        };
    }
    public async Task<MeterUnitsResponse> MeterUnitsData(int id)
    {
        var meterUnits = await _meterUnitsRepo.GetByExpression(x => x.MeterId == id);
        if(meterUnits != null){
            return new MeterUnitsResponse
            {
                Data = meterUnits.Select(x => new GetMeterUnitsDto
                {
                    Id = x.Id,
                    MeterId = x.MeterId,
                    PowerValue = x.PowerValue,
                    VoltageValue = x.VoltageValue,
                    CurrentValue = x.CurrentValue,
                    PowerFactorValue = x.PowerFactorValue,
                    ConsumptionValue = x.ConsumptionValue,
                    ElectricityCost = x.ElectricityCost,
                    TimeValue = x.TimeValue
                }).ToList(),
                Status = true,
                Message = "Meter Units fetched!"
            };
        }
        return new MeterUnitsResponse
        {
            Data = null,
            Status = false,
            Message = "Unable to fetch Meter Units!"
        };
    }
    public async Task<(bool,bool, double)> ResolveUnitAllocation(int meterId, double powerInkWh)
    {
        var meterUnitAllocation = await _meterUnitAllocationRepo.GetByExpression(x => x.MeterId == meterId && (x.unitAllocationStatus == UnitAllocationStatus.Active || x.unitAllocationStatus == UnitAllocationStatus.Pending));
        var meter = await _meterRepo.Get(x => x.Id == meterId);
        var engDiff = 0.00;
        if(meterUnitAllocation != null){
            meterUnitAllocation = meterUnitAllocation.OrderBy(x => x.Id).ToList();
            if(meterUnitAllocation.First().AllocatedUnits < meterUnitAllocation.First().ConsumedUnits){
                engDiff = meterUnitAllocation.First().ConsumedUnits - meterUnitAllocation.First().AllocatedUnits;
                meterUnitAllocation.First().ConsumedUnits -= engDiff;
                meterUnitAllocation.First().unitAllocationStatus = UnitAllocationStatus.Inactive;
                if(DateTime.Today.AddHours(8) < DateTime.Now && DateTime.Today.AddHours(17) >= DateTime.Now){
                    meterUnitAllocation.First().PeakLoad += powerInkWh - engDiff;
                }else{
                    meterUnitAllocation.First().OffPeakLoad += powerInkWh - engDiff;
                }
                await _meterUnitAllocationRepo.Update(meterUnitAllocation.First());
                return (true, true, 0);
            }
            else if(meterUnitAllocation.First().AllocatedUnits > meterUnitAllocation.First().ConsumedUnits){
                meterUnitAllocation.First().unitAllocationStatus = UnitAllocationStatus.Active;
                meterUnitAllocation.First().ConsumedUnits += powerInkWh - engDiff;
                if(DateTime.Today.AddHours(8) < DateTime.Now && DateTime.Today.AddHours(17) >= DateTime.Now){
                    meterUnitAllocation.First().PeakLoad += powerInkWh - engDiff;
                }else{
                    meterUnitAllocation.First().OffPeakLoad += powerInkWh - engDiff;
                }
                await _meterUnitAllocationRepo.Update(meterUnitAllocation.First());
                return (true, true, 0);
            }
            else if(meterUnitAllocation[meterUnitAllocation.IndexOf(meterUnitAllocation.First()) + 1] != null && meterUnitAllocation[meterUnitAllocation.IndexOf(meterUnitAllocation.First()) + 1].unitAllocationStatus == UnitAllocationStatus.Pending && meterUnitAllocation[meterUnitAllocation.IndexOf(meterUnitAllocation.First()) + 1].ConsumedUnits == 0){
                meterUnitAllocation[meterUnitAllocation.IndexOf(meterUnitAllocation.First()) + 1].ConsumedUnits += engDiff;
                meterUnitAllocation[meterUnitAllocation.IndexOf(meterUnitAllocation.First()) + 1].unitAllocationStatus = UnitAllocationStatus.Active;
                if(DateTime.Today.AddHours(8) < DateTime.Now && DateTime.Today.AddHours(17) >= DateTime.Now){
                    meterUnitAllocation[meterUnitAllocation.IndexOf(meterUnitAllocation.First()) + 1].PeakLoad += engDiff;
                    meterUnitAllocation[meterUnitAllocation.IndexOf(meterUnitAllocation.First()) + 1].unitAllocationStatus = UnitAllocationStatus.Active;
                }else{
                    meterUnitAllocation[meterUnitAllocation.IndexOf(meterUnitAllocation.First()) + 1].OffPeakLoad += engDiff;
                }
                await _meterUnitAllocationRepo.Update(meterUnitAllocation.First());
                await _meterUnitAllocationRepo.Update(meterUnitAllocation[meterUnitAllocation.IndexOf(meterUnitAllocation.First()) + 1]);
                return (true, true, 0);
            }
            await CheckUnitsLeftSendPrompt(meterUnitAllocation, meter.MeterId, meter.ConnectionAuth);
            return (false, false, engDiff);
        }
        return (false, false, 0);
    }
    public async Task<bool> CheckUnitsLeftSendPrompt(IList<MeterUnitAllocation> meterUnitAllocations, string meterId, string connectionAuth){
        if(meterUnitAllocations.Count() == 1){
            if(meterUnitAllocations.Last().ConsumedUnits / meterUnitAllocations.Last().AllocatedUnits * 100 < 30){
                var units = meterUnitAllocations.Last().AllocatedUnits - meterUnitAllocations.Last().ConsumedUnits;
                var meterPrompt = new CreateMeterPromptDto{
                    MeterId = meterId,
                    ConnectionAuth = connectionAuth,
                    Title = "Low Units Warning",
                    Description = $"You have only {units}kWh units left. Consider purchasing more units soon.",
                    Type = MeterPromptType.UnitCritical
                };
                await _meterPromptService.CreateMeterPrompt(meterPrompt);
            }
        }
        return true;
    }
    public async Task CheckConnection()
    {
        var meters = await _meterRepo.GetByExpression(x => x.IsActive == true);
        if(meters != null){
            foreach(var meter in meters){
                var meterUnit = await _meterUnitsRepo.GetByExpression(x => x.MeterId == meter.Id);
                if(meterUnit != null && meterUnit.Count() > 0){
                    if(DateTime.Now > meterUnit.Last().TimeValue.AddMinutes(10)){
                        meter.IsActive = false;
                        await _meterRepo.Update(meter);
                    }
                    else{
                        meter.IsActive = true;
                        await _meterRepo.Update(meter);
                    }
                }
                else{
                    meter.IsActive = false;
                    await _meterRepo.Update(meter);
                }
            }
        }
    }
    public async Task<DashBoardResponse> GetDashBoardData()
    {
        var customers = await _cutomerService.GetAllCustomers();
        var meters = await _meterService.GetAllMeters();
        var meterUnits = await _meterUnitAllocationService.GetAllMeterUnitsAllocation();
        if (customers != null || meters != null || meterUnits != null)
        {
            return new DashBoardResponse
            {
                dashboardDto = new DashBoardDto
                {
                    getCustomerDto = customers.Data.ToList(),
                    getMeterDto = meters.Data.ToList(),
                    getTransactionDto = meterUnits.Data.Select(x => x.GetTransactionDto).ToList()
                },
                Status = true
            };
        }
        return new DashBoardResponse
        {
            dashboardDto = null,
            Status = false,
            Message = "Unable to fetch Dashboard Data!"
        };
    }
    public async Task<GetAllTransactionResponse> GetAllTransactions()
    {
        var meterUnits = await _meterUnitAllocationService.GetAllMeterUnitsAllocation();
        if(meterUnits != null)
        {
            return new GetAllTransactionResponse
            {
                Data = meterUnits.Data.Select(x => x.GetTransactionDto).ToList(),
                Status = true
            };
        }
        return new GetAllTransactionResponse
        {
            Data = null,
            Status = false,
            Message = "Unable to fetch Transactions!"
        };
    }
}

