using Charity.Data;
using Charity.Models;
using Charity.Repository.IRepository;

namespace Charity.Repository
{
    public class DonationRepository : Repository<Donation>,IDonationRepository
    {
        private readonly CharityContext _db;
        public DonationRepository(CharityContext db) : base(db)
        {
            _db = db;
        }
    }
}
