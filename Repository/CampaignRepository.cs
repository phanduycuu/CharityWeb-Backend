using Charity.Data;
using Charity.Models;
using Charity.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Charity.Repository
{
    public class CampaignRepository : Repository<Campaign>, ICampaignRepository
    {
        private readonly CharityContext _db;
        public CampaignRepository(CharityContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Campaign> UpdateCurrentAmountAsync(Guid id, decimal amount, string? status = null)
        {
            var existingCampaign = await _db.Campaigns.
               FirstOrDefaultAsync(item => item.Id == id);

            if (existingCampaign == null) return null;
            existingCampaign.CurrentAmount = amount;
            if(status!=null) existingCampaign.Status = status;
            await _db.SaveChangesAsync();
            return existingCampaign;
        }

        public async Task<Campaign> UpdateStatusAmountAsync(Guid id, string status)
        {
            var existingCampaign = await _db.Campaigns.
               FirstOrDefaultAsync(item => item.Id == id);

            if (existingCampaign == null) return null;
            existingCampaign.Status = status;
            await _db.SaveChangesAsync();
            return existingCampaign;
        }
    }
}
