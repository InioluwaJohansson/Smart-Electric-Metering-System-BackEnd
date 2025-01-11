namespace Smart_Electric_Metering_System_BackEnd.Models.DTOs;

public class CreateCustomerDto
{
}
public class UpdateCustomerDto
{
}
public class GetCustomerDto
{
}
public class CustomerResponse : BaseResponse
{
    public GetCustomerDto Data { get; set; }
}
public class CustomersResponse : BaseResponse
{
    public ICollection<GetCustomerDto> Data { get; set; } = new HashSet<GetCustomerDto>();
}