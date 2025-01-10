﻿using Smart_Electric_Metering_System_BackEnd.Entities;

namespace Smart_Metering_System_BackEnd.Interfaces.Repositories;
public interface ICustomerRepo : IGenericRepo<Customer>
{
    public Task<Customer> GetById(int id);
}