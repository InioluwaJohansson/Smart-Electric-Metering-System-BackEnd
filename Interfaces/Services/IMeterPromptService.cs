using Smart_Electric_Metering_System_BackEnd.Models.DTOs;

namespace Smart_Electric_Metering_System_BackEnd.Interfaces.Services;

public interface IMeterPromptService
{
    public Task<BaseResponse> CreateMeterPrompt(CreateMeterPromptDto createMeterPromptDto);
    public Task<BaseResponse> UpdateMeterPrompts(int meterId);
    public Task<MeterPromptsResponse> GetMeterPrompts(int meterId);
}
