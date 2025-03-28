using Smart_Electric_Metering_System_BackEnd.Contracts;
using Smart_Electric_Metering_System_BackEnd.Entities.Identity;
namespace Smart_Electric_Metering_System_BackEnd.Entities;
public class Admin:AuditableEntity
{
    public string AdminId { get; set; } = $"ADMIN{Guid.NewGuid().ToString().Substring(0, 8)}";
    public int UserId { get; set; }
    public User User { get; set; }
}
