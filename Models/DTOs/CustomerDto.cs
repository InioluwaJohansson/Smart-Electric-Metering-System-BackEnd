﻿namespace Smart_Electric_Metering_System_BackEnd.Models.DTOs;

public class CreateCustomerDto
{
    public CreateUserDto createUserDto { get; set; }
}
public class UpdateCustomerDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get;set; }
    public string Picture { get; set; }
    public bool PeakUsageAlerts { get; set; }
    public bool UsageThresholdAlerts { get; set; }
    public bool UsageAlerts { get; set; }
    public bool BillingNotifications { get; set; }
    public bool PushNotifications { get; set; }
}
public class GetCustomerDto
{
    public int Id { get; set; }
    public string CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string PictureUrl { get; set; }
    public List<GetMeterDto> getMeterDto { get; set; }
    public GetNotificationDto getNotificationDto { get; set; }
    public DateTime CreatedOn { get; set; }
}
public class CustomerResponse : BaseResponse
{
    public GetCustomerDto Data { get; set; }
}
public class CustomersResponse : BaseResponse
{
    public ICollection<GetCustomerDto> Data { get; set; } = new HashSet<GetCustomerDto>();
}