using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HelloChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("users")]

        public async Task<IActionResult> Users()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }
    }
}
