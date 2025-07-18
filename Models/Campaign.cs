namespace Charity.Models
{
    public class Campaign
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public decimal GoalAmount { get; set; }
        public decimal CurrentAmount { get; set; } = 0;
        public DateTime Deadline { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Donation> Donations { get; set; }
        public ICollection<CampaignUpdate> Updates { get; set; }
    }
}
