using Smart_Electric_Metering_System_BackEnd.Contracts;
namespace Smart_Electric_Metering_System_BackEnd.Entities;
public class Notification : AuditableEntity
{
    public bool PeakUsageAlerts { get; set; }
    public bool UsageThresholdAlerts { get; set; }
    public bool UsageAlerts { get; set; }
    public bool BillingNotifications { get; set; }
    public bool PushNotifications { get; set; }
}
