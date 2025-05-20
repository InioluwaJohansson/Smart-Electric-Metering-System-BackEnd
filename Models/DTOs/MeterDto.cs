using Smart_Electric_Metering_System_BackEnd.Entities;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;

namespace Smart_Electric_Metering_System_BackEnd.Models.DTOs;

public class CreateMeterDto
{
    public int AdminUserId { get; set; }
    public bool IsActive { get; set; }
}
public class UpdateMeterDto
{
    public int MeterId { get; set; }
    public int UserId { get; set; }
    public double BaseLoad { get; set; }
    public UpdateAddressDto updateAddressDto { get; set; }
}
public class GetMeterDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public string MeterId { get; set; }
    public string MeterKey { get; set; }
    public string ConnectionAuth { get; set; }
    public double TotalUnits { get; set; }
    public double ConsumedUnits { get; set; }
    public double BaseLoad { get; set; }
    public GetAddressDto getAddressDto { get; set; }
    public List<GetMeterUnitAllocationDto> GetMeterUnitAllocationsDto { get; set; }
    public List<GetMeterUnitsDto> GetMeterUnitsDto { get; set; }
    public bool IsActive { get; set; }
    public bool ActiveLoad { get; set; }
    public DateTime DateCreated { get; set; }
}
public class AttachMeterDto{
    public string MeterId { get; set; }
    public string MeterKey { get; set; }
    public int UserId { get; set; }
}
public class ESP32Response : BaseResponse
{
    public double TotalUnits { get; set; }
    public double ConsumedUnits { get; set; }
    public bool IsActive { get; set; }
    public bool ActiveLoad { get; set; }
}
public class MeterResponse : BaseResponse
{
    public GetMeterDto Data { get; set; }
}
public class MetersResponse : BaseResponse
{
    public ICollection<GetMeterDto> Data { get; set; } = new HashSet<GetMeterDto>();
}
