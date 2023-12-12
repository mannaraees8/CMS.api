using CMS.Api.Models;
using CMS.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(LoginUser loginUser)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.RegisterUser(loginUser);
                if (user.Success)
                {
                    return Created($"/auth/user/{user.userData.Id}", loginUser);
                }
                return BadRequest(new { user.Errors });
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUser loginUser)
        {

            if (ModelState.IsValid)
            {
                var user = await _userService.Login(loginUser);
                if (user)
                {
                    var tokenString = _userService.GenerateTokenString(loginUser);
                    return Ok(new {isAuthenticated=true,token= tokenString });
                }
                return Unauthorized (new { isAuthenticated = false,message = "Invalid credentials" });
            }
            return BadRequest(new { isAuthenticated = false, message = "Please fill valid credentials" });
        }
    }
}
