using Smart_Electric_Metering_System_BackEnd.Contracts;
namespace Smart_Electric_Metering_System_BackEnd.Entities;
public class UserDetails : AuditableEntity
{
    public int NIN { get; set; }
    public Address Address { get; set; }
    public bool IsVerified { get; set; }
}
