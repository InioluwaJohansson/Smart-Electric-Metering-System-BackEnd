using Smart_Electric_Metering_System_BackEnd.Contracts;
namespace Smart_Electric_Metering_System_BackEnd.Entities;
public class Prices : AuditableEntity
{
    public string ItemName { get; set; }
    public double Rate { get; set; }
    public double Taxes { get; set; }
}
