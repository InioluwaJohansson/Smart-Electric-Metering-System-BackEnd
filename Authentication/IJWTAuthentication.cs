using System.Security.Claims;
using System.Text;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;
namespace Smart_Electric_Metering_System_BackEnd.Authentication;

public interface IJWTAuthentication
{
    string GenerateToken(GetUserDto getUserDto);
}
