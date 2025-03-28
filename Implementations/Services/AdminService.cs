using Smart_Electric_Metering_System_BackEnd.Entities;
using Smart_Electric_Metering_System_BackEnd.Entities.Identity;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Repositories;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Services;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;

namespace Smart_Electric_Metering_System_BackEnd.Implementations.Services;
public class AdminService : IAdminService
{
    IUserRepo _userRepo;
    IAdminRepo _adminRepo;
    public AdminService(IUserRepo userRepo,IAdminRepo adminRepo)
    {
        _adminRepo = adminRepo;
        _userRepo = userRepo;
    }
    public async Task<BaseResponse> CreateAdmin(CreateAdminDto createAdminDto)
    {
        var Admin = await _userRepo.Get(x => x.UserName == createAdminDto.createUserDto.UserName);
        if(Admin != null)
        {
            return new BaseResponse{
                Status = false,
                Message = "Username Taken!"
            };
        }
        var user = new User{
            FirstName = createAdminDto.createUserDto.FirstName,
            LastName = createAdminDto.createUserDto.LastName,
            UserName = createAdminDto.createUserDto.UserName,
            Password = BCrypt.Net.BCrypt.HashPassword(createAdminDto.createUserDto.Password),
        };
        var userrole = await _userRepo.Create(user);
        var userRole = new UserRole {
            UserId = userrole.Id,
            Role = Role.Admin
        };
        userrole.UserRole = userRole;
        await _userRepo.Update(userrole);
        var NewAdmin =  new Admin {
            AdminId = $"Admin{Guid.NewGuid().ToString().Substring(0,5).ToLower()}",
            UserId = userrole.Id,
        };
        await _adminRepo.Create(NewAdmin);
        return new BaseResponse{
            Status = true,
            Message = "Account Created!"
        };
    }
    public async Task<BaseResponse> UpdateAdmin(UpdateAdminDto updateAdminDto)
    {
        var admin = await _adminRepo.GetById(updateAdminDto.Id);
        if(admin != null)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory() + "..\\..\\Images\\");
            if (!System.IO.Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var imagePath = "";
            if (updateAdminDto.Picture != null)
            {
                var fileName = Path.GetFileNameWithoutExtension(updateAdminDto.Picture.FileName);
                var filePath = Path.Combine(folderPath, updateAdminDto.Picture.FileName);
                var extension = Path.GetExtension(updateAdminDto.Picture.FileName);
                if (!System.IO.Directory.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await updateAdminDto.Picture.CopyToAsync(stream);
                    }
                    imagePath = fileName;
                }
            }
            admin.User.FirstName = updateAdminDto.FirstName ?? admin.User.FirstName;
            admin.User.LastName = updateAdminDto.LastName ?? admin.User.LastName;
            admin.User.PictureUrl = imagePath ?? admin.User.PictureUrl;
            admin.LastModifiedOn = DateTime.Now;
            await _adminRepo.Update(admin);
            return new BaseResponse{
                Status = true,
                Message = "Account Updated!"
            };
        }
        return new BaseResponse{
            Status = false,
            Message = "Unable to find account!"
        };
    }
    public async Task<AdminResponse> GetAdminById(int id){
        var Admin = await _adminRepo.Get(c => c.Id == id);
        if (Admin == null)
        {
            return new AdminResponse
            {
            Status = false,
            Message = "Admin not found!"
            };
        }
        var user = await _userRepo.Get(u => u.Id == Admin.UserId);
        if (user == null)
        {
            return new AdminResponse
            {
            Status = false,
            Message = "User not found!"
            };
        }
        return new AdminResponse
        {
            Status = true,
            Message = "Admin retrieved successfully!",
            Data = await GetAdminDetails(Admin),
        };
    }
    public async Task<AdminsResponse> GetAllAdmins(){
        var admin = await _adminRepo.GetByExpression(x => x.IsDeleted == false);
        if(admin != null){
            return new AdminsResponse{
                Data = (ICollection<GetAdminDto>)admin.Select(x => GetAdminDetails(x)).ToList(),
                Status =  true,
                Message = "admins Data Retrieved!"
            };
        }
        return new AdminsResponse{
            Data = null,
            Status =  false,
            Message = "Unable to Retrieve admins Data!"
        };
    }
    public async Task<GetAdminDto> GetAdminDetails(Admin Admin)
    {
        var user = await _userRepo.Get(u => u.Id == Admin.UserId);
        if (user == null)
        {
            return null;
        }
        return new GetAdminDto
        {
            Id = Admin.Id,
            AdminId = Admin.AdminId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName
        };
    }
    public async Task<BaseResponse> DeleteAdmin(int id){
        var admin = await _adminRepo.Get(c => c.Id == id);
        if (admin == null)
        {
            return new BaseResponse
            {
            Status = false,
            Message = "Admin not found!"
            };
        }
        admin.IsDeleted = true;
        await _adminRepo.Update(admin);
        return new BaseResponse
        {
            Status = true,
            Message = "Admin deleted successfully!",
        };
    }
}
