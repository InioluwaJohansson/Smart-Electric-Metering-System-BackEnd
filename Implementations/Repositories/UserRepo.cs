using Smart_Electric_Metering_System_BackEnd.Context;
using Smart_Electric_Metering_System_BackEnd.Entities.Identity;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Repositories;

namespace Smart_Electric_Metering_System_BackEnd.Implementations.Repositories;

public class UserRepo : GenericRepo<User>, IUserRepo
{
    public UserRepo(SmartElectricMeteringContext _context)
    {
        context = _context;
    }
}
