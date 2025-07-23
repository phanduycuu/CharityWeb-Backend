
using System.ComponentModel.DataAnnotations;

namespace Charity.Dtos.Auth
{
    public class RegisterUserRequest
    {
        [Required]
        public string Name { get; set; }
       
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }
    }

    public class LoginRequestDto
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }

    public class LogoutRequest
    {
        [Required(ErrorMessage = "ID User is required")]
        public Guid IdUser { get; set; }
    }

    public class LoginReponseDto
    {
        public TokenKit TokenKit { get; set; }
        public UserDto User { get; set; } = null!;
    }

    public class TokenKit
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; } = null!;
    }
}
