using Charity.Dtos.Campaign;
using Charity.Dtos.Donation;
using Charity.Dtos.Payment;
using Charity.Helper;

namespace Charity.Service.IService
{
    public interface IDonationServices
    {
        Task<DonationDto> CreateAsync(PaymentRequest paymentDto);
        //Task<QueryObject<CampaignDto>> GetCampaignsAsync(int page, int limit);
        //Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto supplierDto);
        Task<QueryObject<DonationDto>> getByIDCampaignAsync(Guid id, int page, int limit);
    }
}
