
using Charity.Dtos.Auth;
using Charity.Models;


namespace Charity.Service.IService
{
    public interface ITokenService
    {
        Task<TokenKit> GenerateToken(User user);
        Task<User?> InvalidateRefreshToken(Guid idUser);
        Task<TokenKit> RenewToken(TokenKit tokenKit);
    }
}
