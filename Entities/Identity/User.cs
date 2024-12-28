using Smart_Electric_Metering_System.Contracts;
using Smart_Electric_Metering_System.Models.Enums;
namespace Smart_Electric_Metering_System.Entities.Identity;
public class User : AuditableEntity
{
    public int PersonId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string AuthorizationCode { get; set; }
    public UserRole UserRole { get; set; }
}
