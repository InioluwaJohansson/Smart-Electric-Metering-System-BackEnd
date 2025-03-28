using Smart_Electric_Metering_System_BackEnd.Contracts;
namespace Smart_Electric_Metering_System_BackEnd.Entities;
public class MeterUnits : AuditableEntity
{
    public int MeterId { get; set; }
    public double PowerValue { get; set; }
    public double VoltageValue { get; set; }
    public double CurrentValue { get; set; }
    public double ConsumptionValue { get; set; }
    public double ElectricityCost { get; set;}
    public double PowerFactorValue { get; set; }
    public DateTime TimeValue {  get; set; }
}
