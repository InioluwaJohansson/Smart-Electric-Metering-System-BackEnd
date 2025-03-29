namespace Smart_Electric_Metering_System_BackEnd.Models.DTOs;

public class UpdateNotificationDto
{
    public int Id { get; set; }
    public bool PeakUsageAlerts { get; set; }
    public bool UsageThresholdAlerts { get; set; }
    public bool UsageAlerts { get; set; }
    public bool BillingNotifications { get; set; }
    public bool PushNotifications { get; set; }
}
public class GetNotificationDto
{
    public int Id { get; set; }
    public bool PeakUsageAlerts { get; set; }
    public bool UsageThresholdAlerts { get; set; }
    public bool UsageAlerts { get; set; }
    public bool BillingNotifications { get; set; }
    public bool PushNotifications { get; set; }
}