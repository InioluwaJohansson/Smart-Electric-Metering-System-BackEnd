using Smart_Electric_Metering_System_BackEnd.Models.DTOs;

namespace Smart_Electric_Metering_System_BackEnd.Interfaces.Services;

public interface IPricesService
{
    public Task<BaseResponse> UpdatePrices(UpdatePricesDto updatePricesDto);
    public Task<PricesResponse> GetPrices();
}
