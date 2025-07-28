using Charity.Dtos.Campaign;
using Charity.Dtos.Category;
using Charity.Helper;

namespace Charity.Service.IService
{
    public interface ICampaignServices
    {
        Task<CampaignDto> CreateAsync(CreateCampaign categoryDto);
        Task<QueryObject<CampaignDto>> GetCampaignsAsync(int page, int limit);
        //Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto supplierDto);
        Task<CampaignDto?> getByIDAsync(Guid id);
        Task<CampaignDto?> UpdateCurrentAmountasync(Guid id, decimal amount);
        Task<QueryObject<CampaignDto>> searchByKeyAsync(FilterCampaignUserRequest filterCamRequest);
        Task<StatisticalUser> getStatisticalUser();
    }
}
