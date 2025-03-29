namespace Smart_Electric_Metering_System_BackEnd.Models.DTOs;

public class CreateMeterUnitsDto
{
    public string MeterId { get; set; }
    public double PowerValue { get; set; }
    public double VoltageValue { get; set; }
    public double CurrentValue { get; set; }
    public double PowerFactorValue { get; set; }
    public DateTime TimeValue { get; set; }
}
public class GetMeterUnitsDto
{
    public int Id { get; set; }
    public int MeterId { get; set; }
    public double PowerValue { get; set; }
    public double VoltageValue { get; set; }
    public double CurrentValue { get; set; }
    public double ConsumptionValue { get; set; }
    public double ElectricityCost { get; set;}
    public double PowerFactorValue { get; set; }
    public DateTime TimeValue { get; set; }
}
public class MeterUnitResponse : BaseResponse
{
    public GetMeterUnitsDto Data { get; set; }
}
public class MeterUnitsResponse : BaseResponse
{
    public ICollection<GetMeterUnitsDto> Data { get; set; } = new HashSet<GetMeterUnitsDto>();
}