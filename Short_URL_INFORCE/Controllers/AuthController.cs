using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

using Short_URL_INFORCE.Models;

namespace Short_URL_INFORCE.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        // New User Registration
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userDto)
        {
            // Check if user already exists
            var existingUser = await _userManager.FindByNameAsync(userDto.UserName.ToLower());
            if (existingUser != null)
            {
                return BadRequest("User already exists.");
            }

            var user = new IdentityUser { UserName = userDto.UserName };
            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Add role "User"
            await _userManager.AddToRoleAsync(user, "User");

            // Generate JWT Token
            var token = GenerateJwtToken(user);
            return Ok(new { message = "User registered successfully", token });
        }

        // Login Check
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userDto)
        {
            var user = await _userManager.FindByNameAsync(userDto.UserName);
            if (user == null || !(await _userManager.CheckPasswordAsync(user, userDto.Password)))
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        // Generate JWT Token
        private string GenerateJwtToken(IdentityUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: System.DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            //Testing code
            var handler = new JwtSecurityTokenHandler();
            var generatedToken = handler.WriteToken(token);
            Console.WriteLine($"Final Token: {generatedToken}");
            Console.WriteLine(token);
            //
            return new JwtSecurityTokenHandler().WriteToken(token);
            
        }
    }
}

