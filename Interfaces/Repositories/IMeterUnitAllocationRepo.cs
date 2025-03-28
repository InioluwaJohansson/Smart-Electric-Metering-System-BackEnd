using Smart_Electric_Metering_System_BackEnd.Entities;

namespace Smart_Electric_Metering_System_BackEnd.Interfaces.Repositories;

public interface IMeterUnitAllocationRepo : IGenericRepo<MeterUnitAllocation>
{
    public Task<MeterUnitAllocation> GetMeterUnitAllocation(int id);
    public Task<List<MeterUnitAllocation>> GetMeterUnitAllocations(int id);
}
