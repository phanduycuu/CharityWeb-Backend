namespace Charity.Dtos.Payment
{
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public Guid CampaignId { get; set; }
        public Guid? DonorId { get; set; }
        public string DonorName { get; set; }
        public string? DonorEmail { get; set; }
    }
}
