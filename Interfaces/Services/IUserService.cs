using Smart_Electric_Metering_System_BackEnd.Models.DTOs;

namespace Smart_Electric_Metering_System_BackEnd.Interfaces.Services;

public interface IUserService
{
    public Task<bool> CheckUserName(string userName);
    public Task<UserLoginResponse> Login(string username, string password);
    public Task<ForgotPasswordDto> ForgotPassword(string email);
    public Task<BaseResponse> ChangePassword(int id, string username, string password);
    
}
