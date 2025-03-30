using Smart_Electric_Metering_System_BackEnd.Contracts;
using Smart_Metering_System_BackEnd.Models.Enums;
namespace Smart_Electric_Metering_System_BackEnd.Entities;
public class MeterPrompt : AuditableEntity
{
    public int MeterId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public MeterPromptType Type { get; set; }
    public bool IsDismissed { get; set; } = false;
}
