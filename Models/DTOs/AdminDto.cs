namespace Smart_Electric_Metering_System_BackEnd.Models.DTOs;

public class CreateAdminDto
{
    public CreateUserDto createUserDto { get; set; }
}
public class UpdateAdminDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public IFormFile Picture { get; set; }
}
public class GetAdminDto
{
    public int Id { get; set; }
    public string AdminId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string PictureUrl { get; set; }
}
public class AdminResponse : BaseResponse
{
    public GetAdminDto Data { get; set; }
}
public class AdminsResponse : BaseResponse
{
    public ICollection<GetAdminDto> Data { get; set; } = new HashSet<GetAdminDto>();
}