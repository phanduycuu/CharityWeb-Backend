

using Microsoft.AspNetCore.Mvc;
using Charity.Service.IService;
using Charity.Exceptions;
using Charity.Dtos.Auth;
using Google.Apis.Auth;

namespace Charity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthService accountService, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _authService = accountService;
        }

        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenKit tokenKit)
        {
            var result = await _tokenService.RenewToken(tokenKit);
            return Ok(result);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var loginReponse = await _authService.Login(loginRequest);
                if (loginReponse == null) return Unauthorized();

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(1)
                };

                Response.Cookies.Append("accessToken", loginReponse.TokenKit.AccessToken, cookieOptions);
                return Ok(loginReponse);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {

                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest registerRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _authService.Register(registerRequest);
            if (result == null) return BadRequest("Email is already existed!");
            return Ok(result);
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            var result = await _authService.Logout(request.IdUser);
            return Ok(new { message = "Logged out successfully" });
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            try
            {
                // Xác thực token với Google
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);

                // payload chứa email, name, picture,...
                var user = await _authService.LoginWithGoogle(payload);
                if (user == null) return Unauthorized("Invalid Google login");

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(1)
                };

                Response.Cookies.Append("accessToken", user.TokenKit.AccessToken, cookieOptions);
                return Ok(user);
            }
            catch (InvalidJwtException)
            {
                return Unauthorized("Invalid Google token");
            }
        }
    }
}