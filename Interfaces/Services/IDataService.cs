using Smart_Electric_Metering_System_BackEnd.Models.DTOs;

namespace Smart_Electric_Metering_System_BackEnd.Interfaces.Services;

public interface IDataService
{
    public Task<ESP32Response> EstablishConnection(string MeterId, string auth);
    public Task<MeterUnitsResponse> MeterUnitsData(int meterId);
    public Task<BaseResponse> MeterUnitsDataFromESP32(CreateMeterUnitsDto createMeterUnitsDto);
    public Task CheckConnection();
    public Task<DashBoardResponse> GetDashBoardData();
    public Task<GetTransactionPerCustomerResponse> GetTransactionPerCustomer();
}