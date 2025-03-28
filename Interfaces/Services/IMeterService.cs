using Smart_Electric_Metering_System_BackEnd.Models.DTOs;

namespace Smart_Electric_Metering_System_BackEnd.Interfaces.Services;

public interface IMeterService
{
    public Task<BaseResponse> CreateMeter(CreateMeterDto createMeterDto);
    public Task<BaseResponse> AttachMeterToCustomer(AttachMeterDto attachMeterDto);
    public Task<BaseResponse> AllocateMeterUnit(CreateMeterUnitAllocationDto createMeterUnitAllocationDto);
    public Task<MeterResponse> GetMeterById(int meterId);
    public Task<MetersResponse> GetAllMeters();
}
