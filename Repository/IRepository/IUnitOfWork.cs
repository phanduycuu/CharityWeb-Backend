using Microsoft.EntityFrameworkCore.Storage;

namespace Charity.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        IRefreshTokenRepository RefreshToken { get; }
        IUserRepository User { get; }

        Task SaveAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
