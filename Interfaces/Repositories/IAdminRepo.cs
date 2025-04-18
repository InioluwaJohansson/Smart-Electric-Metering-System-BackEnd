﻿using Smart_Electric_Metering_System_BackEnd.Entities;

namespace Smart_Electric_Metering_System_BackEnd.Interfaces.Repositories;

public interface IAdminRepo : IGenericRepo<Admin>
{
    public Task<Admin> GetById(int id);
    public Task<IList<Admin>> GetAdmins();
}
