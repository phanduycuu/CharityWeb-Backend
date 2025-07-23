namespace Charity.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PasswordHash { get; set; } = null!;
        public string Role { get; set; } = "Donor"; // Donor, Admin

        public ICollection<Campaign> CreatedCampaigns { get; set; }
        public ICollection<Donation> Donations { get; set; }
    }
}
