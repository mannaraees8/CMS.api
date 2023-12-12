using CMS.Api.Models;

namespace CMS.Api.Services
{
    public interface IUserService
    {
        Task<UserResponse> RegisterUser(LoginUser loginUser);
        Task<bool> Login(LoginUser loginUser);
        string GenerateTokenString(LoginUser loginUser);
    }
}
