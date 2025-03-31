namespace Smart_Electric_Metering_System_BackEnd.Models.DTOs;
public class GetAddressDto
{
    public int Id { get; set; }
    public string NumberLine { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
}
public class UpdateAddressDto
{
    public int Id { get; set; }
    public string NumberLine { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
}
