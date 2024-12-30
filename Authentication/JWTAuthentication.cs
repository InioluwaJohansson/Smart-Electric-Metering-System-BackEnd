using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using Smart_Electric_Metering_System_BackEnd.Entities.Identity;
namespace Smart_Electric_Metering_System_BackEnd.Authentication;
public class JWTAuthentication : IJWTAuthentication
{
    public string _key;
    public JWTAuthentication(string key)
    {
        _key = key;
    }
    public string GenerateToken(GetUserDto getUserDto)
    {
        List<Claim> claims = new List<Claim>();
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.ASCII.GetBytes(_key);
        claims.Add(new Claim(ClaimTypes.Name, getUserDto.Id.ToString()));
        claims.Add(new Claim(ClaimTypes.Name, getUserDto.PersonId.ToString()));
        claims.Add(new Claim(ClaimTypes.Role, getUserDto.RoleName));
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            IssuedAt = DateTime.Now,
            Expires = DateTime.UtcNow.AddHours(3),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature),
            Audience = "Home",
            Issuer = "Owner",
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}