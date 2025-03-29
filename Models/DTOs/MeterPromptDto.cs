namespace Smart_Electric_Metering_System_BackEnd.Models.DTOs;

public class CreateMeterPromptDto
{
    public string MeterId { get; set; }
    public string ConnectionAuth { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsDismissed { get; set; }
}
public class GetMeterPromptDto
{
    public int Id { get; set; }
    public int MeterId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public bool IsDismissed { get; set; }
}
public class MeterPromptResponse : BaseResponse
{
    public GetMeterPromptDto Data { get; set; }
}
public class MeterPromptsResponse : BaseResponse
{
    public ICollection<GetMeterPromptDto> Data { get; set; } = new HashSet<GetMeterPromptDto>();
}