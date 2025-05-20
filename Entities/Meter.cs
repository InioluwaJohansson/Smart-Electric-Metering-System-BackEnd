using Smart_Electric_Metering_System_BackEnd.Contracts;
namespace Smart_Electric_Metering_System_BackEnd.Entities;
public class Meter : AuditableEntity
{
    public int UserId { get; set; }
    public string MeterId { get; set; } = $"METER{Guid.NewGuid().ToString().Substring(0,8).Replace("-", "").ToUpper()}";
    public string MeterKey { get; set; } 
    public string ConnectionAuth {  get; set; } = Guid.NewGuid().ToString().Substring(0, 18);
    public double TotalUnits { get; set; } = 0.00;
    public double ConsumedUnits { get; set; } = 0.00;
    public double BaseLoad { get; set; } = 0.00;
    public Address MeterAddress { get; set; }
    public List<MeterUnitAllocation> MeterUnitAllocation { get; set; }
    public List<MeterUnits> MeterUnits { get; set; }
    public bool IsActive { get; set; }
    public bool ActiveLoad { get; set; }
}
