using Smart_Electric_Metering_System_BackEnd.Contracts;
namespace Smart_Electric_Metering_System_BackEnd.Entities;
public class Complaint : AuditableEntity
{
    public int CustomerId { get; set; }
    public int MeterId { get; set; }
    public string ComplaintType { get; set; }
    public string Description { get; set; }
    public bool IsResolved { get; set; }
    public bool IsActive { get; set; }
}
