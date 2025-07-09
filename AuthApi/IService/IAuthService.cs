using AuthApi.DTOs;

namespace AuthApi.IService
{
    public interface IAuthService
    {
        Task<(bool success, string message)> RegisterUserAsync( UserSIgnUpDTO request);
        Task<(bool success, string message,  string token)> LoginUserAsync (UserLoginDTO request);
    }
}
