using Smart_Electric_Metering_System_BackEnd.Contracts;
namespace Smart_Electric_Metering_System_BackEnd.Entities;
public class Address: AuditableEntity
{
    public string NumberLine { get; set; } = "";
    public string Street { get; set; } = "";
    public string City { get; set; } = "";
    public string Region { get; set; } = "";
    public string State { get; set; } = "";
    public string Country { get; set; } = "";
}
