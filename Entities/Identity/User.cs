using Smart_Electric_Metering_System_BackEnd.Contracts;
namespace Smart_Electric_Metering_System_BackEnd.Entities.Identity;
public class User : AuditableEntity
{
    public string FirstName { get; set; }
    public string Email { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string PictureUrl { get; set; }
    public string PhoneNumber { get; set; }
    public UserRole UserRole { get; set; }
}
