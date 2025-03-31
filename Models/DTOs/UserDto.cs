namespace Smart_Electric_Metering_System_BackEnd.Models.DTOs;
public class CreateUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
public class GetUserDto
{
    public int Id { get;set;}
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string RoleName { get; set; }
}
public class UserLoginResponse : BaseResponse
{
    public GetUserDto Data { get; set; }
    public string Token { get; set; }
}
public class ForgotPasswordDto : BaseResponse {
    public string username { get; set; } 
    public int id {get; set;}
}
