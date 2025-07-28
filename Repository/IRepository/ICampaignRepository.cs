using Charity.Models;

namespace Charity.Repository.IRepository
{
    public interface ICampaignRepository : IRepository<Campaign>
    {
        Task<Campaign> UpdateCurrentAmountAsync(Guid id, decimal amount, string? status = null);
        Task<Campaign> UpdateStatusAmountAsync(Guid id, string status);
        //Task<Category> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto);
    }
}
