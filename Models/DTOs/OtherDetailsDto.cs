using Smart_Electric_Metering_System_BackEnd.Entities;

namespace Smart_Electric_Metering_System_BackEnd.Models.DTOs;
public class OtherDetailsDto
{
    public int Customerid { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Address Address { get; set; }
    public string PhoneNumber { get; set; }
    public string PictureUrl { get; set; }
}
