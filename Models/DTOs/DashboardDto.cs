using System.Collections.Generic;
using Smart_Electric_Metering_System_BackEnd.Entities;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;

namespace Smart_Electric_Metering_System_BackEnd.Models.DTOs;

public class DashBoardDto
{
    public List<GetTransactionDto> getTransactionDto { get; set; }
    public List<GetMeterDto> getMeterDto { get; set; }
    public List<GetCustomerDto> getCustomerDto { get; set; }
}
public class GetAllTransactionPerCustomer
{
    public string CustomerName { get; set; }
    public List<GetTransactionDto> getTransactionDto { get; set; }
}
public class GetAllTransactionResponse : BaseResponse
{
    public ICollection<GetTransactionDto> Data { get; set; } = new HashSet<GetTransactionDto>();
}
public class DashBoardResponse : BaseResponse
{
    public DashBoardDto dashboardDto { get; set; }
}