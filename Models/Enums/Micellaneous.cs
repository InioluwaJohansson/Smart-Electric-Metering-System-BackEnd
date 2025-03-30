namespace Smart_Metering_System_BackEnd.Models.Enums;
public enum UnitAllocationStatus
{
    Pending = 1, 
    Active,
    Inactive,
}
public enum MeterPromptType{
    PaymentSuccessful = 1,
    UnitCritical,
    VoltageOverload,
}
