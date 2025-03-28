using Smart_Electric_Metering_System_BackEnd.Models.DTOs;

namespace Smart_Electric_Metering_System_BackEnd.Interfaces.Services;

public interface IMeterUnitAllocationService
{
    public Task<BaseResponse> CreateUnitAllocation(int meterId, double amount);
    public Task<MeterUnitAllocationsResponse> GetMeterUnitsAllocation(int meterId);
}
