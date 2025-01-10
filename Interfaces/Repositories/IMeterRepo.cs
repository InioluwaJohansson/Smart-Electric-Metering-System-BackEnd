using Smart_Electric_Metering_System_BackEnd.Entities;

namespace Smart_Metering_System_BackEnd.Interfaces.Repositories;
public interface IMeterRepo : IGenericRepo<Meter>
{
    public Task<Meter> GetMeterById(int id);

}
