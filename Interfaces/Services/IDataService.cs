using Smart_Electric_Metering_System_BackEnd.Models.DTOs;

namespace Smart_Electric_Metering_System_BackEnd.Interfaces.Services;

public interface IDataService
{
    public Task<ESP32Response> EstablishConnection(string MeterId, string auth);
    // public Task<ESP32Response> MeterDataToESP32(string MeterId, string auth);
    public Task<MeterUnitsResponse> MeterUnitsData(int meterId);
    public Task<BaseResponse> MeterUnitsDataFromESP32(CreateMeterUnitsDto createMeterUnitsDto);
}