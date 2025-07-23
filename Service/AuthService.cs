

using Charity.Dtos.Auth;
using Charity.Mapper;
using Charity.Models;
using Charity.Repository.IRepository;
using Charity.Service.IService;



namespace Charity.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        public AuthService(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<UserDto> Register(RegisterUserRequest registerRequest)
        {
            var user = await _unitOfWork.User.GetAsync(u => u.Email == registerRequest.Email);
            if (user != null) return null;
            user = registerRequest.ToRegisterFromCreateDto();
            user.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(registerRequest.Password, 13);
            await _unitOfWork.User.AddAsync(user);
            await _unitOfWork.SaveAsync();
            return user.ToUserDto();
        }

        public async Task<LoginReponseDto> Login(LoginRequestDto loginRequest)
        {
            var user = await _unitOfWork.User.GetAsync(user => 
                user.Email == loginRequest.Email) ?? throw new UnauthorizedAccessException("Account is not existed!");

            if (!BCrypt.Net.BCrypt.EnhancedVerify(loginRequest.Password, user.PasswordHash))
                throw new BadHttpRequestException("Invalid Password");

            TokenKit token = await _tokenService.GenerateToken(user);

            return new LoginReponseDto
            {
                TokenKit = token,
                User = user.ToUserDto(),
            };
        }
        public async Task<User?> Logout(Guid idUser)
        {
            var result = await _tokenService.InvalidateRefreshToken(idUser);
            return result;
        }
    }
}