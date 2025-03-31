using Smart_Electric_Metering_System_BackEnd.Entities;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Repositories;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Services;
using Smart_Metering_System_BackEnd.Models.Enums;

namespace Smart_Electric_Metering_System_BackEnd.Implementations.Services;

public class PricesService : IPricesService
{
    IMeterRepo _meterRepo;
    IPricesRepo _pricesRepo;
    public PricesService(IMeterRepo meterRepo, IPricesRepo pricesRepo)
    {
        _meterRepo = meterRepo;
        _pricesRepo = pricesRepo;
    }
    public async Task<BaseResponse> CreatePrices(CreatePricesDto createPricesDto){
        if(createPricesDto != null){
            var prices = new Prices{
                ItemName = createPricesDto.ItemName,
                BaseCharge = createPricesDto.BaseCharge,
                Rate = createPricesDto.Rate,
                Taxes = createPricesDto.Taxes
            };
            await _pricesRepo.Create(prices);
            return new BaseResponse{
                Status = true,
                Message = "Prices Created!"
            };
        }
        return new BaseResponse{
            Status = false,
            Message = "Unable to create Prices!"
        };
    }
    public async Task<BaseResponse> UpdatePrices(UpdatePricesDto updatePricesDto){
        var prices = await _pricesRepo.Get(x => x.Id == updatePricesDto.Id);
        if(prices != null){
            prices.ItemName = updatePricesDto.ItemName;
            prices.BaseCharge = updatePricesDto.BaseCharge;
            prices.Rate = updatePricesDto.Rate;
            prices.Taxes = updatePricesDto.Taxes;

            await _pricesRepo.Update(prices);
            return new BaseResponse{
                Status = true,
                Message = "Prices Updated!"
            };
        }
        return new BaseResponse{
            Status = false,
            Message = "Unable to update Prices!"
        };
    }
    public async Task<PricesResponse> GetPrices(){
        var prices = await _pricesRepo.Get(x => x.Id == 1);
        if(prices != null){
            return new PricesResponse{
                Data = GetPricesDto(prices),
                Status = true,
                Message = "Data Retrieved!"
            };
        }
        return new PricesResponse{
            Status = false,
            Message = "Unable to retrieve data!"
        };
    }
    public GetPricesDto GetPricesDto(Prices prices){
        return new GetPricesDto{
            Id = prices.Id,
            ItemName = prices.ItemName,
            Rate = prices.Rate,
            Taxes = prices.Taxes,
            BaseCharge = prices.BaseCharge,
        };
    }
}