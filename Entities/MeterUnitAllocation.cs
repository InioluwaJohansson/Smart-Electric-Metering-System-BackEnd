using Smart_Electric_Metering_System_BackEnd.Contracts;
using Smart_Metering_System_BackEnd.Models.Enums;
namespace Smart_Electric_Metering_System_BackEnd.Entities;
public class MeterUnitAllocation : AuditableEntity
{
    public int MeterId { get; set; }
    public double AllocatedUnits { get; set; }
    public double ConsumedUnits { get; set; }
    public double BaseLoad { get; set; }
    public double PeakLoad { get; set; }
    public double OffPeakLoad { get; set; }
    public Transaction Transaction { get; set; }
    public UnitAllocationStatus unitAllocationStatus { get; set; } = UnitAllocationStatus.Pending;
}
