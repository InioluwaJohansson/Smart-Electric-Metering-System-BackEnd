using System.Security.Claims;
using System.Text;
using Smart_Electric_Metering_System.Models.DTOs;
namespace Smart_Electric_Metering_System.Authentication;

public interface IJWTAuthentication
{
    string GenerateToken(GetUserDto getUserDto);
}
