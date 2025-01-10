using Smart_Electric_Metering_System_BackEnd.Contracts;
namespace Smart_Electric_Metering_System_BackEnd.Entities;
public class MeterUnitAllocation : AuditableEntity
{
    public int? MeterId { get; set; }
    public double AllocatedUnits { get; set; }
    public double ConsumedUnits { get; set; }
    public bool IsActive { get; set; }
}
