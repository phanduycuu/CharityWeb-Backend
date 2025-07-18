namespace Charity.Models
{
    public class Subscription
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
    }
}
