using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySongs.Common.DTOs;
using MySongs.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MySongs.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Email))
                return BadRequest("נא להזין אימייל");
            if (string.IsNullOrWhiteSpace(userDto.Password))
                return BadRequest("נא להזין סיסמה");
            if (userDto.Password.Length < 6)
                return BadRequest("הסיסמה חייבת להכיל לפחות 6 תווים");
            if (string.IsNullOrWhiteSpace(userDto.Username))
                return BadRequest("נא להזין שם משתמש");

            var allUsers = await _userService.GetAll();
            var existing = allUsers.FirstOrDefault(u => u.Email == userDto.Email);
            if (existing != null)
                return BadRequest("משתמש עם אימייל זה כבר קיים");

            userDto.Role = "User";
            await _userService.Add(userDto);
            return Ok("נרשמת בהצלחה!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            var allUsers = await _userService.GetAll();
            var user = allUsers.FirstOrDefault(u => u.Email == userDto.Email);
            if (user == null)
                return Unauthorized("אימייל או סיסמה שגויים");
            if (user.Password != userDto.Password)
                return Unauthorized("אימייל או סיסמה שגויים");

            var token = GenerateToken(user);
            return Ok(new { token, userId = user.UserId, userName = user.Username, role = user.Role });
        }

        private string GenerateToken(UserDto user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, user.Username ?? ""),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}