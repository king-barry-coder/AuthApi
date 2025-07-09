using AuthApi.DTOs;
using AuthApi.IService;
using AuthApi.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public object SecurityAlogrithm { get; private set; }

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<(bool success, string message)> RegisterUserAsync(UserSIgnUpDTO request)
        {
            var ExistingUser = await _userManager.FindByEmailAsync(request.Email);
            if (ExistingUser != null)
            {
                return (false, "User with this email already exist");
            }

            var NewUser = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email
            };

            var IsCreated = await _userManager.CreateAsync(NewUser, request.Password);
            if (IsCreated.Succeeded)
            {
                return (true, "User Successful ");
            }
            return (false, "Failed Creation");
        }

        public async Task<(bool success, string message, string token)> LoginUserAsync(UserLoginDTO request)
        {
            var ExistingUser = await _userManager.FindByEmailAsync(request.Email);
            if (ExistingUser != null)
            {
                return (false, "user not found", null);
            }

            var IsCorrect = await _userManager.CheckPasswordAsync(ExistingUser,request.Password);
            if (!IsCorrect) 
            {
                return (false, "Wrong Credentials", null);
            }

            var token = GenerateJwtToken(ExistingUser);
            return(true, "Login Successful", token);
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:secret"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var Claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName + "" + user.LastName),
            };

            var token = new JwtSecurityToken
                (
                 _configuration["Jwt: Issues"],
                 _configuration["Jwt: Audience"],
                 Claims,
                 expires: DateTime.UtcNow.AddHours(2),
                 signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
    }
}
