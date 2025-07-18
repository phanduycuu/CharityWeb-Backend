namespace Charity.Models
{
    public class Donation
    {
        public Guid Id { get; set; }
        public Guid CampaignId { get; set; }
        public Campaign Campaign { get; set; }

        public decimal Amount { get; set; }
        public DateTime DonatedAt { get; set; } = DateTime.UtcNow;

        public Guid? DonorId { get; set; }
        public User? Donor { get; set; }

        public string? DonorName { get; set; }
        public string? DonorEmail { get; set; }
    }
}
