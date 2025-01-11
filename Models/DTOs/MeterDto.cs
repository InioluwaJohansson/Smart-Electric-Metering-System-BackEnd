using Smart_Electric_Metering_System_BackEnd.Entities;

namespace Smart_Electric_Metering_System_BackEnd.Models.DTOs;

public class CreateMeterDto
{
    public int AdminUserId { get; set; }
    public bool IsActive { get; set; }
}
