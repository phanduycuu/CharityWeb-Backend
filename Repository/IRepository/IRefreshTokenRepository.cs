
using Charity.Dtos.Category;
using Charity.Models;

namespace Charity.Repository.IRepository
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task UpdateRevokedAsync(RefreshToken storedToken);
    }
}
