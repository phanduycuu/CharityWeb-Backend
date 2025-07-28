using Charity.Models;

namespace Charity.Dtos.Campaign
{
    public class CreateCampaign
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public IFormFile ImageUrl { get; set; }
        public decimal GoalAmount { get; set; }
        public DateTime Deadline { get; set; }
        public Guid CreatedById { get; set; }
        public Guid CategoryId { get; set; }
    }
}
