using Smart_Electric_Metering_System_BackEnd.Models.DTOs;

namespace Smart_Electric_Metering_System_BackEnd.Models.DTOs;
public class CreateTransactionDto{
    public double Rate { get; set; }
    public double BaseCharge { get; set; }
    public double Taxes { get; set; }
    public double Total { get; set; }
}
public class GetTransactionDto{
    public DateTime Date { get; set; }
    public DateTime Time { get; set; }
    public string TransactionId { get; set; }
    public double Rate { get; set; }
    public double BaseCharge { get; set; }
    public double Taxes { get; set; }
    public double Total { get; set; }
}