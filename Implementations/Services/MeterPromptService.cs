using Smart_Electric_Metering_System_BackEnd.Entities;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Repositories;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Services;
using Smart_Metering_System_BackEnd.Models.Enums;

namespace Smart_Electric_Metering_System_BackEnd.Implementations.Services;

public class MeterPromptService : IMeterPromptService
{
    IMeterRepo _meterRepo;
    IMeterPromptRepo _meterPromptRepo;
    public MeterPromptService(IMeterRepo meterRepo, IMeterPromptRepo meterPromptRepo)
    {
        _meterRepo = meterRepo;
        _meterPromptRepo = meterPromptRepo;
    }
    public async Task<BaseResponse> CreateMeterPrompt(CreateMeterPromptDto createMeterPromptDto){
        var meter = await _meterRepo.Get(x => x.MeterId == createMeterPromptDto.MeterId && x.ConnectionAuth == createMeterPromptDto.ConnectionAuth);
        if(createMeterPromptDto != null)
        {
            var meterPrompt = new MeterPrompt()
            {
                MeterId = meter.Id,
                Title = createMeterPromptDto.Title,
                Description = createMeterPromptDto.Description,
                Date = DateTime.Now,
                IsDismissed = false,
            };
            await _meterPromptRepo.Create(meterPrompt);
            return new BaseResponse
            {
                Status = true,
                Message = "Meter Prompt Successfully added!"
            };
        }
        return new BaseResponse
        {
            Status = false,
            Message = "Error adding Meter Prompt!"
        };
    }
    public async Task<BaseResponse> UpdateMeterPrompts(int meterId){
        var meterPrompts = await _meterPromptRepo.GetByExpression(x => x.MeterId == meterId && x.IsDismissed == false);
        if(meterPrompts != null){
            foreach(var meterPrompt in meterPrompts){
                meterPrompt.IsDismissed = true;
                await _meterPromptRepo.Update(meterPrompt);
            }
            return new BaseResponse{
                Status = true,
                Message = "Meter Prompts Updated!"
            };
        }
        return new BaseResponse{
            Status = false,
            Message = "Unable to update Meter Prompts!"
        };
    }
    public async Task<MeterPromptsResponse> GetMeterPrompts(int meterId){
        var meterPrompts = await _meterPromptRepo.GetByExpression(x => x.MeterId == meterId);
        if(meterPrompts != null){
            return new MeterPromptsResponse{
                Data = meterPrompts.Select(x => GetMeterPromptDto(x)).ToList(),
                Status = true,
                Message = "Data Retrieved!"
            };
        }
        return new MeterPromptsResponse{
            Status = false,
            Message = "Unable to retrieve data!"
        };
    }
    public GetMeterPromptDto GetMeterPromptDto(MeterPrompt meterPrompt){
        return new GetMeterPromptDto{
            Id = meterPrompt.Id,
            MeterId = meterPrompt.MeterId,
            Title = meterPrompt.Title,
            Description = meterPrompt.Description,
            Date = meterPrompt.Date,
            IsDismissed = meterPrompt.IsDismissed,
        };
    }
}