using Smart_Electric_Metering_System_BackEnd.Entities.Identity;

namespace Smart_Electric_Metering_System_BackEnd.Interfaces.Repositories;

public interface IUserRepo : IGenericRepo<User>
{
   public Task<User> GetUserById(string username);
}
