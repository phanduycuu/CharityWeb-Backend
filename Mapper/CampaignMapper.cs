

using Charity.Dtos.Campaign;
using Charity.Models;

namespace Charity.Mapper
{
    public static class CampaignMapper
    {
        public static CampaignDto ToCampaignDto(this Campaign Campaign)
        {
            return new CampaignDto
            {
                Id = Campaign.Id,
                Title = Campaign.Title,
                ImageUrl = Campaign.ImageUrl,
                Description = Campaign.Description,
                GoalAmount = Campaign.GoalAmount,
                CurrentAmount = Campaign.CurrentAmount,
                Deadline = Campaign.Deadline,
                CreatedBy = Campaign?.CreatedBy?.ToUserDto(),
                Category = Campaign?.Category?.ToCategoryDto(),
                Status=Campaign?.Status,
                Donations = []

            };
        }

        public static Campaign ToCampaignFromCreateDto(this CreateCampaign createCampaignDto)
        {
            return new Campaign
            {
                 Title = createCampaignDto.Title,
                 Description= createCampaignDto.Description,
                 GoalAmount=createCampaignDto.GoalAmount,
                 CurrentAmount = 0,
                 Deadline =createCampaignDto.Deadline,
                 CreatedById =createCampaignDto.CreatedById,
                 CategoryId =createCampaignDto.CategoryId,
                 Donations = []
    };
        }
    }
}
