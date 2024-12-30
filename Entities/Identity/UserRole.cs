using Smart_Electric_Metering_System_BackEnd.Contracts;

namespace Smart_Electric_Metering_System_BackEnd.Entities.Identity;

public class UserRole : AuditableEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
    public Role Role { get; set; }
}
