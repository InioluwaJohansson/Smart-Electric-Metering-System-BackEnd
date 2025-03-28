using Smart_Electric_Metering_System_BackEnd.Models.DTOs;

namespace Smart_Electric_Metering_System_BackEnd.Interfaces.Services;

public interface ICustomerService
{
    public Task<BaseResponse> CreateCustomer(CreateCustomerDto createCustomerDto);
    public Task<BaseResponse> UpdateCustomer(UpdateCustomerDto updateCustomerDto);
    public Task<CustomerResponse> GetCustomerById(int id);
    public Task<CustomersResponse> GetAllCustomers();
}
