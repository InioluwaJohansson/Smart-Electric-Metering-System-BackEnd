using Microsoft.AspNetCore.Mvc;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Services;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Smart_Metering_System_BackEnd.Controllers
{
    [Route("SEMS/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _userService;
        UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userService.Login(username, password);
            if (user.Status == true)
            {
                return Ok(user);
            }
            return Ok(user);
        }
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userService.ForgotPassword(email);
            if (user.Item3.Status == true)
            {
                return Ok(user);
            }
            return Ok(user);
        }
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(int id, string username, string password)
        {
            var user = await _userService.ChangePassword(id, username, password);
            if (user.Status == true)
            {
                return Ok(user);
            }
            return Ok(user);
        }
    }
}
