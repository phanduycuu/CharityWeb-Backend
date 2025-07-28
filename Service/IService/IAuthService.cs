
using Charity.Dtos.Auth;
using Charity.Models;
using Google.Apis.Auth;

namespace Charity.Service.IService
{
    public interface IAuthService
    {
        Task<UserDto> Register(RegisterUserRequest userDto);
        Task<LoginReponseDto>  Login(LoginRequestDto loginRequest);
        Task<User> Logout(Guid userId);
        Task<LoginReponseDto> LoginWithGoogle(GoogleJsonWebSignature.Payload payload);
    }
}
