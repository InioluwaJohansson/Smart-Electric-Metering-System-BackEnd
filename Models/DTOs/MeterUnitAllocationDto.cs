using Smart_Electric_Metering_System_BackEnd.Models.DTOs;
using Smart_Metering_System_BackEnd.Models.Enums;

namespace Smart_Electric_Metering_System_BackEnd.Models.DTOs;

public class CreateMeterUnitAllocationDto
{
    public int MeterId { get; set; }
    public double AllocatedUnits { get; set; }
    public double ConsumedUnits { get; set; } = 0;
    public bool IsActive { get; set; }
}
public class GetMeterUnitAllocationDto
{
    public int Id { get; set; }
    public int MeterId { get; set; }
    public double AllocatedUnits { get; set; }
    public double ConsumedUnits { get; set; }
    public double BaseLoad { get; set; }
    public double PeakLoad { get; set; }
    public double OffPeakLoad { get; set; }
    public GetTransactionDto GetTransactionDto { get; set; }
    public UnitAllocationStatus unitAllocationStatus { get; set; }
}
public class MeterUnitAllocationResponse : BaseResponse
{
    public GetMeterUnitAllocationDto Data { get; set; }
}
public class MeterUnitAllocationsResponse : BaseResponse
{
    public ICollection<GetMeterUnitAllocationDto> Data { get; set; } = new HashSet<GetMeterUnitAllocationDto>();
}
