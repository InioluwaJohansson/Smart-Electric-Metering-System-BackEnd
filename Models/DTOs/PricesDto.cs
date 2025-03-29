namespace Smart_Electric_Metering_System_BackEnd.Models.DTOs;

public class UpdatePricesDto
{
    public int Id { get; set; }
    public string ItemName { get; set; }
    public double Rate { get; set; }
    public double Taxes { get; set; }
    public double BaseCharge { get; set; }
}
public class GetPricesDto
{
    public int Id { get; set; }
    public string ItemName { get; set; }
    public double Rate { get; set; }
    public double Taxes { get; set; }
    public double BaseCharge { get; set; }
}
public class PricesResponse : BaseResponse
{
    public GetPricesDto Data { get; set; }
}