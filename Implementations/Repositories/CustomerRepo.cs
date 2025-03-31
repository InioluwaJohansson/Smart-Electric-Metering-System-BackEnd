using Microsoft.EntityFrameworkCore;
using Smart_Electric_Metering_System_BackEnd.Context;
using Smart_Electric_Metering_System_BackEnd.Entities;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Repositories;

namespace Smart_Electric_Metering_System_BackEnd.Implementations.Repositories;

public class CustomerRepo : GenericRepo<Customer>, ICustomerRepo
{
    public CustomerRepo(SmartElectricMeteringContext _context)
    {
        context = _context;
    }
    public async Task<Customer> GetById(int id)
    {
        return await context.Customers.Include(x => x.User).Include(x => x.Notification).FirstOrDefaultAsync(x => x.User.Id == id  && x.IsDeleted == false);
    }
    public async Task<IList<Customer>> GetCustomers()
    {
        return await context.Customers.Include(x => x.User).Include(x => x.Notification).Where(x => x.IsDeleted == false).ToListAsync();
    }
}