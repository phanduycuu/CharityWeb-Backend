using Charity.Dtos.Campaign;
using Charity.Dtos.Category;
using Charity.Helper;
using Charity.Mapper;
using Charity.Models;
using Charity.Repository.IRepository;
using Charity.Service.IService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Charity.Service
{
    public class CampaignServices : ICampaignServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        public CampaignServices(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public async Task<CampaignDto> CreateAsync(CreateCampaign campaignCreateDto)
        {
            Campaign campaign = campaignCreateDto.ToCampaignFromCreateDto();
            string imageUrl = "";
            if (campaignCreateDto.ImageUrl != null)
            {
                await _imageService.SetDirect("images/campaign");
                imageUrl = await _imageService.HandleImageUploadAsync(campaignCreateDto.ImageUrl);
                campaign.ImageUrl = imageUrl;
            }
            
            campaign.Deadline = DateTime.SpecifyKind(campaign.Deadline, DateTimeKind.Utc);
            await _unitOfWork.Campaign.AddAsync(campaign);
            await _unitOfWork.SaveAsync();
            return campaign.ToCampaignDto();
        }
        public async Task<CampaignDto?> getByIDAsync(Guid id)
        {
            Campaign? campaignExisting = await _unitOfWork.Campaign.GetAsync(c => c.Id == id, includeProperties: "Category,CreatedBy");
            if (campaignExisting != null)
                return campaignExisting.ToCampaignDto();

            return null;
        }

        public async Task<QueryObject<CampaignDto>> GetCampaignsAsync(int page, int limit)
        {
            var campaigns = await _unitOfWork.Campaign.GetAllAsync(includeProperties: "Category,CreatedBy");
            var campaignsDto = campaigns.Select(c =>
                c.ToCampaignDto()).FilterPage(page, limit);
            return campaignsDto;
        }

        public async Task<CampaignDto?> UpdateCurrentAmountasync(Guid id, decimal amount)
        {
            Campaign? campaignExisting = await _unitOfWork.Campaign.GetAsync(c => c.Id == id, includeProperties: "Category,CreatedBy");
            decimal CurrentAmount = campaignExisting.CurrentAmount+amount;
            Campaign campaign =null;
            if (CurrentAmount >= campaignExisting.GoalAmount) {
                campaign= await _unitOfWork.Campaign.UpdateCurrentAmountAsync(id, CurrentAmount, "Finished");
            }
            else {  campaign = await _unitOfWork.Campaign.UpdateCurrentAmountAsync(id, CurrentAmount); }
            return campaign.ToCampaignDto();
        }

        public async Task<QueryObject<CampaignDto>> searchByKeyAsync(FilterCampaignUserRequest filterCamRequest)
        {
            var campaigns = await _unitOfWork.Campaign.GetAllAsync(includeProperties:
                                 "Category,CreatedBy");

            var campaignQuery = campaigns.AsQueryable().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filterCamRequest.SearchKey))
                campaignQuery = campaignQuery.Where(p => p.Title.Contains(filterCamRequest.SearchKey));

            if (filterCamRequest.CategoryId!=null)
                campaignQuery = campaignQuery.Where(p => p.Category.Id == filterCamRequest.CategoryId);


            var rs = campaignQuery.Select(p => p.ToCampaignDto()).FilterPage(filterCamRequest.Page ?? 1, filterCamRequest.Limit);
            return rs;
        }


        public async Task<StatisticalUser> getStatisticalUser()
        {
            var campaigns = await _unitOfWork.Campaign.GetAllAsync();
            var donation = await _unitOfWork.Donation.GetAllAsync();
            StatisticalUser statistical = new StatisticalUser();
            statistical.TotalCampaign = campaigns.Count();
            statistical.ToltalDonation=donation.Count();
            statistical.TotalAmount= donation.Sum(p => p.Amount);
            return statistical;
        }
    }
}
