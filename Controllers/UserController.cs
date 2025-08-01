using Microsoft.AspNetCore.Mvc;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Services;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;
using Smart_Electric_Metering_System_BackEnd.Authentication;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Smart_Metering_System_BackEnd.Controllers
{
    [Route("SEMS/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _userService;
        IJWTAuthentication _auth;
        public UserController(IUserService userService, IJWTAuthentication auth)
        {
            _userService = userService;
            _auth = auth;
        }
        [HttpGet("CheckUserName")]
        public async Task<IActionResult> CheckUserName(string username)
        {
            var user = await _userService.CheckUserName(username);
            if (user == true)
            {
                return Ok(user);
            }
            return Ok(user);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userService.Login(username, password);
            if (user.Status == true)
            {
                var token = _auth.GenerateToken(user.Data);
                user.Token = token;
                return Ok(user);
            }
            return Ok(user);
        }
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userService.ForgotPassword(email);
            if (user.Status == true)
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
