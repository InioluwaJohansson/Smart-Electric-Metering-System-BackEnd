using Smart_Electric_Metering_System_BackEnd.Entities;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Repositories;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Services;
using Smart_Metering_System_BackEnd.Models.Enums;

namespace Smart_Electric_Metering_System_BackEnd.Implementations.Services;

public class MeterUnitAllocationService : IMeterUnitAllocationService
{
    IMeterRepo _meterRepo;
    IMeterUnitAllocationRepo _meterUnitAllocationRepo;
    IPricesRepo _pricesRepo;
    IMeterPromptService _meterPromptService;
    public MeterUnitAllocationService(IMeterRepo meterRepo, IMeterUnitAllocationRepo meterUnitAllocationRepo, IPricesRepo pricesRepo, IMeterPromptService meterPromptService)
    {
        _meterRepo = meterRepo;
        _meterUnitAllocationRepo = meterUnitAllocationRepo;
        _pricesRepo = pricesRepo;
        _meterPromptService = meterPromptService;
    }
    public async Task<BaseResponse> CreateUnitAllocation(int meterId, double amount){
        var meter = await _meterRepo.Get(x => x.Id == meterId);
        var prices = await _pricesRepo.Get(x => x.Id == 1);
        if(meter != null && prices != null){
            var meterUnit = new MeterUnitAllocation {
                MeterId = meter.Id,
                AllocatedUnits = amount,
                ConsumedUnits = 0.00,
                unitAllocationStatus = UnitAllocationStatus.Pending,
                BaseLoad = meter.BaseLoad,
                Transaction = new Transaction{
                    TransactionId = Guid.NewGuid().ToString().Substring(0,16).Replace("-", "").ToUpper(),
                    Rate = prices.Rate,
                    BaseCharge = prices.BaseCharge,
                    Taxes = prices.Taxes * prices.Rate * amount / 100,
                    Total = (amount * prices.Rate) + (prices.Taxes * prices.Rate * amount / 100) + prices.BaseCharge,
                },
            };
            await _meterUnitAllocationRepo.Create(meterUnit);
            meter.TotalUnits += amount;
            await _meterRepo.Update(meter);
            var meterPrompt = new CreateMeterPromptDto{
                MeterId = meter.MeterId,
                ConnectionAuth = meter.ConnectionAuth,
                Title = "Bill Payment Successful",
                Description = $"Your payment of #{meterUnit.Transaction.Total} was successfully processed.",
                Type = MeterPromptType.PaymentSuccessful
            };
            await _meterPromptService.CreateMeterPrompt(meterPrompt);
            return new BaseResponse{
                Status = true,
                Message = "Meter Allocation Successful!"
            };
        }
        return new BaseResponse{
            Status = false,
            Message = "Unable To Find Meter"
        };
    }
    public async Task<MeterUnitAllocationsResponse> GetMeterUnitsAllocation(int meterId){
        var meterUnits = await _meterUnitAllocationRepo.GetMeterUnitAllocations(meterId);
        if(meterUnits != null){
            return new MeterUnitAllocationsResponse{
                Data = meterUnits.Select(x => GetMeterUnitAllocationDto(x)).ToList(),
                Status = true,
                Message = "Data Retrieved!"
            };
        }
        return new MeterUnitAllocationsResponse{
            Data = null,
            Status = false,
            Message = "Unable to retrieve data!"
        };
    }
    public async Task<MeterUnitAllocationsResponse> GetAllMeterUnitsAllocation(){
        var meterUnits = await _meterUnitAllocationRepo.GetAllMeterUnitAllocations();
        if(meterUnits != null){
            return new MeterUnitAllocationsResponse{
                Data = meterUnits.Select(x => GetMeterUnitAllocationDto(x)).ToList(),
                Status = true,
                Message = "Data Retrieved!"
            };
        }
        return new MeterUnitAllocationsResponse{
            Data = null,
            Status = false,
            Message = "Unable to retrieve data!"
        };
    }
    public GetMeterUnitAllocationDto GetMeterUnitAllocationDto(MeterUnitAllocation meterUnitAllocation){
        return new GetMeterUnitAllocationDto{
            Id = meterUnitAllocation.Id,
            MeterId = meterUnitAllocation.MeterId,
            AllocatedUnits = meterUnitAllocation.AllocatedUnits,
            ConsumedUnits = meterUnitAllocation.ConsumedUnits,
            BaseLoad = meterUnitAllocation.BaseLoad,
            PeakLoad = meterUnitAllocation.PeakLoad,
            OffPeakLoad = meterUnitAllocation.OffPeakLoad,
            unitAllocationStatus = meterUnitAllocation.unitAllocationStatus.ToString(),
            GetTransactionDto = new GetTransactionDto{
                TransactionId = meterUnitAllocation.Transaction.TransactionId,
                Date = meterUnitAllocation.Transaction.Date,
                Time = meterUnitAllocation.Transaction.Time,
                Rate = meterUnitAllocation.Transaction.Rate,
                BaseCharge = meterUnitAllocation.Transaction.BaseCharge,
                Taxes = meterUnitAllocation.Transaction.Taxes,
                Total = meterUnitAllocation.Transaction.Total
            },
        };
    }
}