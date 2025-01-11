using Smart_Electric_Metering_System_BackEnd.Entities;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;
using Smart_Metering_System_BackEnd.Interfaces.Repositories;
using Smart_Metering_System_BackEnd.Interfaces.Services;

namespace Smart_Metering_System_BackEnd.Implementations.Services;

public class MeterService : IMeterService
{
    IMeterRepo _meterRepo;
    IMeterUnitsRepo _meterUnitsRepo;
    IMeterUnitAllocationRepo _meterUnitAllocationRepo;
    IUserRepo _userRepo;
    MeterService(IMeterRepo meterRepo, IMeterUnitsRepo meterUnitsRepo, IMeterUnitAllocationRepo meterUnitAllocationRepo, IUserRepo userRepo)
    {
        _meterRepo = meterRepo;
        _meterUnitsRepo = meterUnitsRepo;
        _meterUnitAllocationRepo = meterUnitAllocationRepo;
        _userRepo = userRepo;
    }
    public async Task<BaseResponse> EstablishConnection(string MeterId, string auth)
    {
        var meter = await _meterRepo.Get(x => x.MeterId == $"METER{MeterId}" && x.ConnectionAuth == auth);
        if (meter != null)
        {
            meter.IsActive = true;
            await _meterRepo.Update(meter);
            return new BaseResponse()
            {
                Status = true,
                Message = "Connection Handshake Established!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable to establish Connection!"
        };
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
                IsDeleted = false,
                UserId = 0
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
    public async Task<BaseResponse> AttachMeterToCustomer(int userId, int meterId, string MeterKey)
    {
        var user = await _userRepo.Get(x => x.Id == userId);
        if (user != null)
        {
            var meter = await _meterRepo.Get(x => x.Id == meterId && x.MeterKey == MeterKey);
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
    public async Task<BaseResponse> AllocateMeterUnit(AllocateMeterUnitDto allocateMeterUnitDto)
    {
        if (allocateMeterUnitDto != null)
        {
            var meterUnit = await _meterUnitsRepo.Get(x => x.MeterUnitId == allocateMeterUnitDto.MeterUnitId);
            if (meterUnit != null)
            {
                var meterUnitAllocation = new MeterUnitAllocation()
                {
                    MeterId = allocateMeterUnitDto.MeterId,
                    MeterUnitId = allocateMeterUnitDto.MeterUnitId,
                    IsActive = true,
                    CreatedBy = allocateMeterUnitDto.AdminUserId,
                    LastModifiedBy = allocateMeterUnitDto.AdminUserId,
                    IsDeleted = false
                };
                await _meterUnitAllocationRepo.Create(meterUnitAllocation);
                return new BaseResponse
                {
                    Status = true,
                    Message = "Meter Unit Successfully Allocated!"
                };
            }
            return new BaseResponse
            {
                Status = false,
                Message = "Meter Unit not found!"
            };
        }
        return new BaseResponse
        {
            Status = false,
            Message = "Error Allocating Meter Unit!"
        };
    }

}
