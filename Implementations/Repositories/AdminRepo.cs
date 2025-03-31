using Microsoft.EntityFrameworkCore;
using Smart_Electric_Metering_System_BackEnd.Context;
using Smart_Electric_Metering_System_BackEnd.Entities;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Repositories;

namespace Smart_Electric_Metering_System_BackEnd.Implementations.Repositories;

public class AdminRepo : GenericRepo<Admin>, IAdminRepo
{
    public AdminRepo(SmartElectricMeteringContext _context)
    {
        context = _context;
    }
    public async Task<Admin> GetById(int id)
    {
        return await context.Admins.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Id == id  && x.IsDeleted == false);
    }
    public async Task<IList<Admin>> GetAdmins()
    {
        return await context.Admins.Include(x => x.User).Where(x => x.IsDeleted == false).ToListAsync();
    }
}

