using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HorsesForCourses.Api.Auth;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _cfg;

    public AuthController(IConfiguration cfg)
    { _cfg = cfg; }

    [HttpPost("token")]
    public async Task<IActionResult> Token(LoginDto dto)
    {
        // var user = await _users.ValidateAsync(dto.Email, dto.Password);
        // if (user is null) return Unauthorized();

        var claims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, "user.Name"),
        new Claim(ClaimTypes.Name, "user.Email"),
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["Auth:JwtKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: _cfg["Auth:Issuer"],
            audience: _cfg["Auth:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        return Ok(new { access_token = token, token_type = "Bearer", expires_in = 3600 });
    }
}

public record LoginDto(string Email, string Password);