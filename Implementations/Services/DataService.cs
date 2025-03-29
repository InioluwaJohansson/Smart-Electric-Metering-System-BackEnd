using Smart_Electric_Metering_System_BackEnd.Entities;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Repositories;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Services;
using Smart_Metering_System_BackEnd.Models.Enums;

namespace Smart_Electric_Metering_System_BackEnd.Implementations.Services;

public class DataService : IDataService
{
    IMeterRepo _meterRepo;
    IMeterUnitsRepo _meterUnitsRepo;
    IMeterUnitAllocationRepo _meterUnitAllocationRepo;
    IUserRepo _userRepo;
    IPricesRepo _pricesRepo;
    public DataService(IMeterRepo meterRepo, IMeterUnitsRepo meterUnitsRepo, IMeterUnitAllocationRepo meterUnitAllocationRepo, IUserRepo userRepo, IPricesRepo pricesRepo)
    {
        _meterRepo = meterRepo;
        _meterUnitsRepo = meterUnitsRepo;
        _meterUnitAllocationRepo = meterUnitAllocationRepo;
        _userRepo = userRepo;
        _pricesRepo = pricesRepo;
    }
    public async Task<ESP32Response> EstablishConnection(string MeterId, string auth)
    {
        var meter = await _meterRepo.Get(x => x.MeterId == $"METER{MeterId}" && x.ConnectionAuth == auth);
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
    public async Task<ESP32Response> MeterDataToESP32(string MeterId, string auth)
    {
        var meter = await _meterRepo.Get(x => x.MeterId == $"METER{MeterId}" && x.ConnectionAuth == auth);
        if(meter != null)
        {
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
            Message = "Unable to send Data to ESP32"
        };
    }
    public async Task<BaseResponse> MeterUnitsDataFromESP32(CreateMeterUnitsDto createMeterUnitsDto){
        var meter = await _meterRepo.Get(x => x.MeterId == $"METER{createMeterUnitsDto.MeterId}");
        if(meter != null && meter.TotalUnits > meter.ConsumedUnits){
            var powerInkWh = createMeterUnitsDto.PowerValue * 0.001 / 360;
            var meterUnitAllocationResolve = await ResolveUnitAllocation(meter.Id, powerInkWh, createMeterUnitsDto);
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
                PowerFactorValue = createMeterUnitsDto.PowerFactorValue,
                TimeValue = createMeterUnitsDto.TimeValue,
                ConsumptionValue = powerInkWh,
                ElectricityCost = (await _pricesRepo.Get(x => x.Id == 1)).Rate * powerInkWh,
            };
            if(meterUnitAllocationResolve.Item1 == false && meterUnitAllocationResolve.Item2 == false && meterUnitAllocationResolve.Item3 == 0){
                unit = null;
            }
            await _meterRepo.Update(meter);
            if(unit != null){
                await _meterUnitsRepo.Create(unit);
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
        var meterUnits = await _meterUnitsRepo.GetByExpression(x => x.Id == id);
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
    public async Task<(bool,bool, double)> ResolveUnitAllocation(int meterId, double powerInkWh, CreateMeterUnitsDto createMeterUnitsDto)
    {
        var meterUnitAllocation = await _meterUnitAllocationRepo.GetByExpression(x => x.MeterId == meterId && (x.unitAllocationStatus == UnitAllocationStatus.Active || x.unitAllocationStatus == UnitAllocationStatus.Pending));
        var engDiff = 0.00;
        if(meterUnitAllocation != null){
            meterUnitAllocation = meterUnitAllocation.OrderByDescending(x => x).ToList();
            if(meterUnitAllocation.First().AllocatedUnits < meterUnitAllocation.First().ConsumedUnits){
                engDiff = meterUnitAllocation.First().ConsumedUnits - meterUnitAllocation.First().AllocatedUnits;
                meterUnitAllocation.First().ConsumedUnits -= engDiff;
                meterUnitAllocation.First().unitAllocationStatus = UnitAllocationStatus.Inactive;
                if(DateTime.Today.AddHours(8) < createMeterUnitsDto.TimeValue && DateTime.Today.AddHours(17) >= createMeterUnitsDto.TimeValue){
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
                if(DateTime.Today.AddHours(8) < createMeterUnitsDto.TimeValue && DateTime.Today.AddHours(17) >= createMeterUnitsDto.TimeValue){
                    meterUnitAllocation.First().PeakLoad += engDiff;
                }else{
                    meterUnitAllocation.First().OffPeakLoad += engDiff;
                }
                await _meterUnitAllocationRepo.Update(meterUnitAllocation.First());
                await _meterUnitAllocationRepo.Update(meterUnitAllocation[meterUnitAllocation.IndexOf(meterUnitAllocation.First()) + 1]);
                return (true, true, 0);
            }
            return (false, false, engDiff);
        }
        return (false, false, 0);
    }
}