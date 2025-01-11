using Smart_Electric_Metering_System_BackEnd.Contracts;
namespace Smart_Electric_Metering_System_BackEnd.Entities;
public class MeterUnits : AuditableEntity
{
    public int MeterId { get; set; }
    public double PowerValue { get; set; }
    public DateTime TimeValue {  get; set; }
}
