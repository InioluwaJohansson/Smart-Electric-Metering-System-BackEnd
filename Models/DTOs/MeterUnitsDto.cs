using Smart_Electric_Metering_System_BackEnd.Models.DTOs;

namespace Smart_Metering_System_BackEnd.Models.DTOs;

public class CreateMeterUnitsDto
{
}
public class GetMeterUnitsDto
{
}
public class MeterUnitResponse : BaseResponse
{
    public GetMeterUnitsDto Data { get; set; }
}
public class MeterUnitsResponse : BaseResponse
{
    public ICollection<GetMeterUnitsDto> Data { get; set; } = new HashSet<GetMeterUnitsDto>();
}