namespace Charity.Models
{
    public class CampaignUpdate
    {
        public Guid Id { get; set; }

        public Guid CampaignId { get; set; }
        public Campaign Campaign { get; set; }

        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
