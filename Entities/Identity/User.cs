using Smart_Electric_Metering_System_BackEnd.Contracts;
using Smart_Electric_Metering_System_BackEnd.Models.Enums;
namespace Smart_Electric_Metering_System_BackEnd.Entities.Identity;
public class User : AuditableEntity
{
    public int PersonId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string AuthorizationCode { get; set; }
    public UserRole UserRole { get; set; }
}
