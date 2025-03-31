using Smart_Electric_Metering_System_BackEnd.Entities;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Repositories;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Services;
using Smart_Metering_System_BackEnd.Models.Enums;

namespace Smart_Electric_Metering_System_BackEnd.Implementations.Services;

public class MeterService : IMeterService
{
    IMeterRepo _meterRepo;
    IUserRepo _userRepo;
    public MeterService(IMeterRepo meterRepo, IUserRepo userRepo)
    {
        _meterRepo = meterRepo;
        _userRepo = userRepo;
    }
    public async Task<BaseResponse> CreateMeter(CreateMeterDto createMeterDto)
    {
        if(createMeterDto != null)
        {
            var meter = new Meter()
            {
                IsActive = false,
                CreatedBy = createMeterDto.AdminUserId,
                LastModifiedBy = createMeterDto.AdminUserId,
                MeterKey = $"METER{Guid.NewGuid().ToString().Substring(0,8).Replace("-", "").ToUpper()}",
                IsDeleted = false,
                UserId = 0,
                MeterAddress = new Address{
                    Country = "",
                    City = "",
                    State = "",
                    Region = "",
                    Street = "",
                    NumberLine = "",
                }
            };
            await _meterRepo.Create(meter);
            return new BaseResponse
            {
                Status = true,
                Message = "Meter Successfully added!"
            };
        }
        return new BaseResponse
        {
            Status = false,
            Message = "Error adding Meter!"
        };
    }
    public async Task<BaseResponse> AttachMeterToCustomer(AttachMeterDto attachMeterDto)
    {
        var user = await _userRepo.Get(x => x.Id == attachMeterDto.UserId);
        if (user != null)
        {
            var meter = await _meterRepo.Get(x => x.MeterId == attachMeterDto.MeterId && x.MeterKey == attachMeterDto.MeterKey);
            if (meter != null && meter.UserId == 0)
            {
                meter.UserId = user.Id;
                await _meterRepo.Update(meter);
                return new BaseResponse
                {
                    Status = true,
                    Message = "Meter Successfully attached to Customer!"
                };
            }
            return new BaseResponse
            {
                Status = false,
                Message = "Meter not found!"
            };
        }
        return new BaseResponse
        {
            Status = false,
            Message = "Error attaching Meter to Customer!"
        };
    }
    public async Task<BaseResponse> UpdateMeter(UpdateMeterDto updateMeterDto)
    {
        var user = await _userRepo.Get(x => x.Id == updateMeterDto.UserId);
        if (user != null)
        {
            var meter = await _meterRepo.Get(x => x.MeterId == updateMeterDto.MeterId);
            if (meter != null)
            {
                meter.BaseLoad = updateMeterDto.BaseLoad;
                meter.IsActive = updateMeterDto.IsActive;
                meter.MeterAddress.Country = updateMeterDto.updateAddressDto.Country ?? meter.MeterAddress.Country;
                meter.MeterAddress.State = updateMeterDto.updateAddressDto.State ?? meter.MeterAddress.State;
                meter.MeterAddress.Region = updateMeterDto.updateAddressDto.Region ?? meter.MeterAddress.Region;
                meter.MeterAddress.City = updateMeterDto.updateAddressDto.City ?? meter.MeterAddress.City;
                meter.MeterAddress.Street = updateMeterDto.updateAddressDto.Street ?? meter.MeterAddress.Street;
                meter.MeterAddress.NumberLine = updateMeterDto.updateAddressDto.NumberLine ?? meter.MeterAddress.NumberLine;
                await _meterRepo.Update(meter);
                return new BaseResponse
                {
                    Status = true,
                    Message = "Meter Information Updated!"
                };
            }
            return new BaseResponse
            {
                Status = false,
                Message = "Meter not found!"
            };
        }
        return new BaseResponse
        {
            Status = false,
            Message = "Error Finding User!"
        };
    }
    public async Task<MeterResponse> GetMeterById(int meterId)
    {
        var meter = await _meterRepo.GetMeterById(meterId);
        if (meter != null)
        {
            return new MeterResponse
            {
                Status = true,
                Message = "Meter Found!",
                Data = await GetMeterDto(meter)
            };
        }
        return new MeterResponse
        {
            Status = false,
            Message = "Meter not found!"
        };
    }
    public async Task<MetersResponse> GetAllMeters()
    {
        var meters = await _meterRepo.GetAllMeters();
        if (meters != null)
        {
            List<GetMeterDto> meterList = new List<GetMeterDto>();
            foreach (var item in meters) meterList.Add(await GetMeterDto(item));
            return new MetersResponse
            {
                Status = true,
                Message = "Meters Found!",
                Data = meterList,
            };
        }
        return new MetersResponse
        {
            Status = false,
            Message = "No Meters Found!"
        };
    }
    public async Task<GetMeterDto> GetMeterDto(Meter meter)
    {
        var getuser = await _userRepo.Get(x => x.Id == meter.UserId);
        var customerName = "Meter not yet attached";
        if (getuser.FirstName != null)  customerName = getuser.FirstName + " " + getuser.LastName;
        return new GetMeterDto
        {
            Id = meter.Id,
            CustomerName = customerName,
            MeterId = meter.MeterId,
            MeterKey = meter.MeterKey,
            ConnectionAuth = meter.ConnectionAuth,
            BaseLoad = meter.BaseLoad,
            TotalUnits = meter.MeterUnitAllocation.Sum(x => x.AllocatedUnits),
            ConsumedUnits = meter.MeterUnitAllocation.Sum(x => x.ConsumedUnits),
            getAddressDto = new GetAddressDto () {
                NumberLine = meter.MeterAddress.City,
                Street = meter.MeterAddress.Street,
                City = meter.MeterAddress.City,
                Region = meter.MeterAddress.Region,
                State = meter.MeterAddress.State
            },
            GetMeterUnitAllocationsDto = meter.MeterUnitAllocation.Select(x => new GetMeterUnitAllocationDto
            {
                Id = x.Id,
                AllocatedUnits = x.AllocatedUnits,
                ConsumedUnits = x.ConsumedUnits,
                BaseLoad = x.BaseLoad,
                PeakLoad = x.PeakLoad,
                OffPeakLoad = x.OffPeakLoad,
                MeterId = x.MeterId,
                unitAllocationStatus = x.unitAllocationStatus
            }).ToList(),
            GetMeterUnitsDto = meter.MeterUnits.Select(x => new GetMeterUnitsDto
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
            IsActive = meter.IsActive
        };
    }
}
