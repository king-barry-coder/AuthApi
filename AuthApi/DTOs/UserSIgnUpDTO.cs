using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTOs
{
    public class UserSIgnUpDTO
    {
        // the require is for data annotation 
        [Required, MinLength(3)]
        public string FirstName { get; set; }

        [Required, MinLength(3)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        [Required, Compare("Password", ErrorMessage = "Password Mismatch")]
        public string ConfirmPaasword { get; set; }  
    }
}
