namespace Smart_Electric_Metering_System_BackEnd.Contracts;

public interface IAuditableEntity
{
    public int CreatedBy{get; set;}
    public DateTime CreatedOn{get; set;}
    public int LastModifiedBy{get; set;}
    public DateTime? LastModifiedOn{get; set;}
}