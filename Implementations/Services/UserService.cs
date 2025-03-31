using Smart_Electric_Metering_System_BackEnd.Interfaces.Repositories;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Services;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;

namespace Smart_Electric_Metering_System_BackEnd.Implementations.Services;
public class UserService : IUserService
{
    IUserRepo _userRepo;
    public UserService(IUserRepo userRepo)
    {
        _userRepo = userRepo;
    }
    public async Task<bool> CheckUserName(string userName){
        var user = await _userRepo.Get(x => x.UserName.StartsWith(userName));
        if(user == null) return true;
        return false;
    }
    public async Task<UserLoginResponse> Login(string username, string password)
    {
        var user = await _userRepo.GetUserById(username);
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return new UserLoginResponse()
            {
                Data = new GetUserDto()
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    RoleName = user.UserRole.Role.ToString(),
                },
                Message = "Login Successful",
                Status = true,
            };
        }
        return new UserLoginResponse()
        {
            Data = null,
            Message = "Invalid Username Or Password!",
            Status = false
        };
    }
    public async Task<(string, int, BaseResponse)> ForgotPassword(string email)
    {
        var user = await _userRepo.Get(c => c.Email == email);
        if (user != null)
        {
            return ($"{user.UserName}", user.Id,  new BaseResponse{
                Status = true,
                Message = "Valid Email!"
            });
        }
        return ("", 0, new BaseResponse()
        {
            Message = "Invalid Email!",
            Status = false
        });
    }
    public async Task<BaseResponse> ChangePassword(int id, string username, string password)
    {
        var user = await _userRepo.Get(x => x.UserName == username);
        var userWithId = await _userRepo.Get(x => x.Id == id);
        if (user != null && userWithId != null && user == userWithId)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(password);
            user.LastModifiedOn = DateTime.Now;
            await _userRepo.Update(user);
            return new BaseResponse()
            {
                Message = "Password Changed Successfully!",
                Status = true
            };
        }
        return new BaseResponse()
        {
            Message = "Error!",
            Status = false
        };
    }
}