using Microsoft.EntityFrameworkCore;
using Smart_Electric_Metering_System_BackEnd.Context;
using Smart_Electric_Metering_System_BackEnd.Entities;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Repositories;

namespace Smart_Electric_Metering_System_BackEnd.Implementations.Repositories;

public class MeterUnitAllocationRepo : GenericRepo<MeterUnitAllocation>, IMeterUnitAllocationRepo
{
    public MeterUnitAllocationRepo(SmartElectricMeteringContext _context)
    {
        context = _context;
    }
    public async Task<MeterUnitAllocation> GetMeterUnitAllocation(int id){
        return await context.MeterUnitAllocations.Include(x => x.Transaction).SingleOrDefaultAsync(x => x.Id == id);
    }
    public async Task<List<MeterUnitAllocation>> GetMeterUnitAllocations(int id){
        return await context.MeterUnitAllocations.Include(x => x.Transaction).Where(x => x.MeterId == id).ToListAsync();
    }
}
