using Charity.Dtos.Category;
using Charity.Models;

namespace Charity.Dtos.Campaign
{
    public class CampaignDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public decimal GoalAmount { get; set; }
        public decimal CurrentAmount { get; set; } = 0;
        public DateTime Deadline { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public UserDto? CreatedBy { get; set; }
        public CategoryDto? Category { get; set; }
        public string Status { get; set; }
        public ICollection<Models.Donation> Donations { get; set; } = new List<Models.Donation>();
        //public ICollection<CampaignUpdate> Updates { get; set; }
    }
}
