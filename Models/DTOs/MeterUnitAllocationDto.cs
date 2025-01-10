using Smart_Electric_Metering_System_BackEnd.Models.DTOs;

namespace Smart_Metering_System_BackEnd.Models.DTOs;

public class CreateMeterUnitAllocationDto
{
}
public class GetMeterUnitAllocationDto
{
}
public class MeterUnitAllocationResponse : BaseResponse
{
    public GetMeterUnitAllocationDto Data { get; set; }
}
public class MeterUnitAllocationsResponse : BaseResponse
{
    public ICollection<GetMeterUnitAllocationDto> Data { get; set; } = new HashSet<GetMeterUnitAllocationDto>();
}
