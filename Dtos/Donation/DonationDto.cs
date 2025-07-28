using Charity.Dtos.Campaign;
using Charity.Models;

namespace Charity.Dtos.Donation
{
    public class DonationDto
    {
        public Guid Id { get; set; }
        public CampaignDto? Campaign { get; set; }

        public decimal Amount { get; set; }
        public DateTime DonatedAt { get; set; } = DateTime.UtcNow;
        public UserDto? Donor { get; set; }
        public string? DonorName { get; set; }
        public string? DonorEmail { get; set; }
    }
}
