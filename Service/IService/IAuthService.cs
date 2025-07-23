
using Charity.Dtos.Auth;
using Charity.Models;

namespace Charity.Service.IService
{
    public interface IAuthService
    {
        Task<UserDto> Register(RegisterUserRequest userDto);
        Task<LoginReponseDto>  Login(LoginRequestDto loginRequest);
        Task<User> Logout(Guid userId);
    }
}
