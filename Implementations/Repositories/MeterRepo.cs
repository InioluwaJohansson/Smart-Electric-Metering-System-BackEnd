using Microsoft.EntityFrameworkCore;
using Smart_Electric_Metering_System_BackEnd.Context;
using Smart_Electric_Metering_System_BackEnd.Entities;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Repositories;

namespace Smart_Electric_Metering_System_BackEnd.Implementations.Repositories;
public class MeterRepo : GenericRepo<Meter>, IMeterRepo
{
    public MeterRepo(SmartElectricMeteringContext _context)
    {
        context = _context;
    }
    public async Task<Meter> GetMeterById(int id)
    {
        return await context.Meters.Include(x => x.MeterAddress).Include(x => x.MeterUnitAllocation).ThenInclude(x => x.Transaction).Include(x => x.MeterUnits).SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
    }
    public async Task<IList<Meter>> GetAllMeters()
    {
        return await context.Meters.Include(x => x.MeterAddress).Include(x => x.MeterUnitAllocation).ThenInclude(x => x.Transaction).Include(x => x.MeterUnits).Where(x => x.IsDeleted == false).ToListAsync();
    }
}
