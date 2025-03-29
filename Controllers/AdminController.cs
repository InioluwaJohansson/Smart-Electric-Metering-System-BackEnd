using Microsoft.AspNetCore.Mvc;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Services;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Smart_Metering_System_BackEnd.Controllers
{
    [Route("SEMS/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        IAdminService _adminService;
        AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpPost("CreateAdmin")]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminDto createAdminDto)
        {
            var admin = await _adminService.CreateAdmin(createAdminDto);
            if (admin.Status == true)
            {
                return Ok(admin);
            }
            return Ok(admin);
        }
        [HttpPut("UpdateAdmin")]
        public async Task<IActionResult> UpdateAdmin([FromBody] UpdateAdminDto updateAdminDto)
        {
            var admin = await _adminService.UpdateAdmin(updateAdminDto);
            if (admin.Status == true)
            {
                return Ok(admin);
            }
            return Ok(admin);
        }
        // GET: api/<AdminController>
        [HttpGet("GetAdminById{id}")]
        public async Task<IActionResult> GetAdminById(int id)
        {
            var admin = await _adminService.GetAdminById(id);
            if (admin.Status == true)
            {
                return Ok(admin);
            }
            return Ok(admin);
        }
        [HttpGet("GetAllAdmins")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admin = await _adminService.GetAllAdmins();
            if (admin.Status == true)
            {
                return Ok(admin);
            }
            return Ok(admin);
        }
        [HttpPut("DeleteAdmin{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var admin = await _adminService.DeleteAdmin(id);
            if (admin.Status == true)
            {
                return Ok(admin);
            }
            return Ok(admin);
        }
    }
}
