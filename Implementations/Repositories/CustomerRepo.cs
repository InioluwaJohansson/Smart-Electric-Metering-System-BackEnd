﻿using Microsoft.EntityFrameworkCore;
using Smart_Electric_Metering_System_BackEnd.Context;
using Smart_Electric_Metering_System_BackEnd.Entities;
using Smart_Metering_System_BackEnd.Interfaces.Repositories;

namespace Smart_Metering_System_BackEnd.Implementations.Repositories;

public class CustomerRepo : GenericRepo<Customer>, ICustomerRepo
{
    CustomerRepo(SmartElectricMeteringContext _context)
    {
        context = _context;
    }
    public async Task<Customer> GetById(int id)
    {
        return await context.Customers.Include(x => x.UserDetails).Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
    }
}
