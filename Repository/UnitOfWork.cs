using Charity.Data;
using Charity.Repository.IRepository;
using Microsoft.EntityFrameworkCore.Storage;


namespace Charity.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly CharityContext _db;
        private IDbContextTransaction _transaction;

        public ICategoryRepository Category { get; }
        public IRefreshTokenRepository RefreshToken { get; }

        public IUserRepository User { get; }
        public ICampaignRepository Campaign { get; }
        public IDonationRepository Donation { get; }
        public UnitOfWork(CharityContext db)
        {
            _db = db;

            Category = new CategoryRepository(db);
            User = new UserRepository(db);
            RefreshToken = new RefreshTokenRepository(db);
            Campaign= new CampaignRepository(db);
            Donation = new DonationRepository(db);

        }
       
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }


        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            _transaction = await _db.Database.BeginTransactionAsync();
            return _transaction;
        }



        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _db.Dispose();
        }
    }
}
