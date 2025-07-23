using Microsoft.EntityFrameworkCore;
using Charity.Models;
using Charity.Repository.IRepository;
using Charity.Data;

namespace Charity.Repository
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly CharityContext _db;
        public RefreshTokenRepository(CharityContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateRevokedAsync(RefreshToken storedToken)
        {
            var token = await _db.RefreshTokens.FirstOrDefaultAsync(t => t.Token == storedToken.Token);
            token.IsRevoked = true;
            token.IsUsed = true; 
            await _db.SaveChangesAsync();
        }
    }
}
