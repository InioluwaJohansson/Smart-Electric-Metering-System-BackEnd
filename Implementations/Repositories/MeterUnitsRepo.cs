﻿using Smart_Electric_Metering_System_BackEnd.Context;
using Smart_Electric_Metering_System_BackEnd.Entities;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Repositories;

namespace Smart_Electric_Metering_System_BackEnd.Implementations.Repositories;

public class MeterUnitsRepo: GenericRepo<MeterUnits>, IMeterUnitsRepo
{
    public MeterUnitsRepo(SmartElectricMeteringContext _context)
    {
        context = _context;
    }
}
