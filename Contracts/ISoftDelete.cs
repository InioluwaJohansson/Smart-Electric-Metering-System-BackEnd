namespace Smart_Electric_Metering_System.Contracts;

public interface ISoftDelete
{
     DateTime? DeletedOn{get; set;}
     int DeletedBy{get; set;}
     bool IsDeleted{get; set;}
}