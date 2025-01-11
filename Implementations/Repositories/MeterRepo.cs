using Microsoft.EntityFrameworkCore;
using Smart_Electric_Metering_System_BackEnd.Context;
using Smart_Electric_Metering_System_BackEnd.Entities;
using Smart_Metering_System_BackEnd.Interfaces.Repositories;

namespace Smart_Metering_System_BackEnd.Implementations.Repositories;
public class MeterRepo : GenericRepo<Meter>, IMeterRepo
{
    MeterRepo(SmartElectricMeteringContext _context)
    {
        context = _context;
    }
    public async Task<Meter> GetMeterById(int id)
    {
        return await context.Meters.Include(x => x.MeterUnitAllocation).Include(x => x.MeterUnits).SingleOrDefaultAsync(x => x.Id == id);
    }
}
