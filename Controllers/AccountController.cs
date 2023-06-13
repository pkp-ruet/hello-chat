using HelloChat.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HelloChat.Services;

namespace HelloChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AuthenticationConfiguration _authConfig;
        private readonly IMailService _mailService;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AuthenticationConfiguration authConfig, IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authConfig = authConfig;
            _mailService = mailService;
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var user = new IdentityUser
            {
                UserName = registerRequest.Username,
                Email = registerRequest.Email,
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            if (result.Succeeded)
            {
                await SendMail(user);
                return Ok("Registration Success");
            }
            return BadRequest();
        }

        private async Task SendMail(IdentityUser user)
        {
            var mailRequest = new MailRequest
            {
                ToEmail = user.Email,
                Subject = "Registration"
            };
            await _mailService.SendMailAsync(mailRequest);
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if(!ModelState.IsValid)
                return BadRequest();
            var user = await _userManager.FindByNameAsync(loginRequest.Username);
            if (user == null)
                return BadRequest();
            var result =
                await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (result)
            {
                var token = GenerateAccessToken(user);
                return Ok(token);
            }
            return BadRequest("Invalid request");

        }
        public string GenerateAccessToken(IdentityUser user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authConfig.AccessTokenSecret));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
                _authConfig.Issuer, _authConfig.Audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(_authConfig.AccessTokenExpirationTime),
                credentials);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}
