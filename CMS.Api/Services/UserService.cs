using CMS.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CMS.Api.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;
        public UserService(UserManager<IdentityUser> userManager,IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }
        public async Task<UserResponse> RegisterUser(LoginUser loginUser)
        {
            var identityUser = new IdentityUser
            {
                UserName = loginUser.Email,
                Email = loginUser.Email
            };
            var result = await _userManager.CreateAsync(identityUser, loginUser.Password);
            if (result.Succeeded)
            {
                var successResponse = new UserResponse
                {
                    Success = true,
                    Message = "New user has been registered.",
                    userData = new UserData
                    {
                        Id = identityUser.Id
                    }
                };
                return successResponse;
            }
            var response = new UserResponse
            {
                Success = false,
                Message = "Registration failed. Please check your input.",
                Errors= result.Errors.Select(e => e.Description)
            };
            return response;
        }
        public async Task<bool> Login(LoginUser loginUser)
        {
            var identityUser = await _userManager.FindByEmailAsync(loginUser.Email);
            if (identityUser is null)
            {
                return false;
            }
            var user = await _userManager.CheckPasswordAsync(identityUser, loginUser.Password);
            return user;
        }
        public string GenerateTokenString(LoginUser loginUser)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,loginUser.Email),
                new Claim(ClaimTypes.Role,"Admin"),
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
            var signinCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                issuer: _config.GetSection("Jwt:Issuer").Value,
                audience: _config.GetSection("Jwt:Audience").Value,
                signingCredentials:signinCred);
            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }

    }
}
