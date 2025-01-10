﻿using Smart_Electric_Metering_System_BackEnd.Context;
using Smart_Electric_Metering_System_BackEnd.Entities;
using Smart_Metering_System_BackEnd.Interfaces.Repositories;

namespace Smart_Metering_System_BackEnd.Implementations.Repositories;

public class AdminRepo : GenericRepo<Admin>, IAdminRepo
{
    AdminRepo(SmartElectricMeteringContext _context)
    {
        context = _context;
    }
}
