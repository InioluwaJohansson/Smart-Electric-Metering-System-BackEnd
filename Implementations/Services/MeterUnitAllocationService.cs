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
    public MeterUnitAllocationService(IMeterRepo meterRepo, IMeterUnitAllocationRepo meterUnitAllocationRepo, IPricesRepo pricesRepo)
    {
        _meterRepo = meterRepo;
        _meterUnitAllocationRepo = meterUnitAllocationRepo;
        _pricesRepo = pricesRepo;
    }
    public async Task<BaseResponse> CreateUnitAllocation(int meterId, double amount){
        var meter = await _meterRepo.Get(x => x.Id == meterId);
        var prices = await _pricesRepo.Get(x => x.Id == 1);
        if(meter != null){
            var meterUnit = new MeterUnitAllocation {
                MeterId = meter.Id,
                AllocatedUnits = amount,
                ConsumedUnits = 0.00,
                unitAllocationStatus = UnitAllocationStatus.Pending,
                BaseLoad = meter.BaseLoad / 30,
                Transaction = new Transaction{
                    Rate = prices.Rate,
                    BaseCharge = amount * prices.Rate,
                    Taxes = prices.Taxes * prices.Rate * amount / 100,
                    Total = (amount * prices.Rate) + (prices.Taxes * prices.Rate * amount / 100),
                },
            };
            await _meterUnitAllocationRepo.Create(meterUnit);
            meter.TotalUnits += amount;
            await _meterRepo.Update(meter);
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
    public GetMeterUnitAllocationDto GetMeterUnitAllocationDto(MeterUnitAllocation meterUnitAllocation){
        return new GetMeterUnitAllocationDto{
            Id = meterUnitAllocation.Id,
            MeterId = meterUnitAllocation.MeterId,
            AllocatedUnits = meterUnitAllocation.AllocatedUnits,
            ConsumedUnits = meterUnitAllocation.ConsumedUnits,
            BaseLoad = meterUnitAllocation.BaseLoad,
            PeakLoad = meterUnitAllocation.PeakLoad,
            OffPeakLoad = meterUnitAllocation.OffPeakLoad,
            unitAllocationStatus = meterUnitAllocation.unitAllocationStatus,
            GetTransactionDto = new GetTransactionDto{
                TransactionId = meterUnitAllocation.Transaction.TransactionId,
                Date = meterUnitAllocation.Transaction.Date,
                Time = meterUnitAllocation.Transaction.Time,
                Rate = meterUnitAllocation.Transaction.Rate,
                BaseCharge = meterUnitAllocation.Transaction.BaseCharge,
                Taxes = meterUnitAllocation.Transaction.Taxes,
                Total = meterUnitAllocation.Transaction.Taxes
            },
        };
    }
}