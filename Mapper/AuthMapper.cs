using Charity.Models;
using Charity.Dtos.Auth;

namespace Charity.Mapper
{
    public static class AuthMapper
    {
        public static User ToRegisterFromCreateDto(this RegisterUserRequest registerRequest)
        {
            return new User
            {
                FullName = registerRequest.Name,
                Email = registerRequest.Email,
                PasswordHash = registerRequest.Password,
                Role = registerRequest.Role
            };
        }
    }
}
