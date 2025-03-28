using Smart_Electric_Metering_System_BackEnd.Models.DTOs;

namespace Smart_Electric_Metering_System_BackEnd.Interfaces.Services;

public interface IAdminService
{
    public Task<BaseResponse> CreateAdmin(CreateAdminDto createAdminDto);
    public Task<BaseResponse> UpdateAdmin(UpdateAdminDto updateAdminDto);
    public Task<AdminResponse> GetAdminById(int id);
    public Task<AdminsResponse> GetAllAdmins();
    public Task<BaseResponse> DeleteAdmin(int id);
}
