using Smart_Electric_Metering_System_BackEnd.Contracts;
namespace Smart_Electric_Metering_System_BackEnd.Entities;
public class Transaction : AuditableEntity
{
    public DateTime Date { get; set; } = DateTime.Today;
    public DateTime Time { get; set; } = DateTime.UtcNow;
    public string TransactionId  { get; set; } = Guid.NewGuid().ToString().Substring(0,12);
    public double Rate { get; set; }
    public double BaseCharge { get; set; }
    public double Taxes { get; set; }
    public double Total { get; set; }
}