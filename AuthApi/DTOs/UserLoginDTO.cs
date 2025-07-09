using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTOs
{
    public class UserLoginDTO
    {
        [ Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }
    }
}
