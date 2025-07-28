

using Charity.Dtos.Campaign;
using Charity.Dtos.Donation;
using Charity.Dtos.Payment;
using Charity.Models;

namespace Charity.Mapper
{
    public static class DonationMapper
    {
        public static DonationDto ToDonationDto(this Donation donation)
        {
            return new DonationDto
            {
                Id = donation.Id,
                Amount = donation.Amount,
                Campaign = donation?.Campaign?.ToCampaignDto(),
                Donor = donation?.Donor?.ToUserDto(),
                DonorName = donation?.DonorName,
                DonorEmail = donation?.DonorEmail,
                DonatedAt = donation.DonatedAt,


            };
        }

        public static Donation ToPaymentFromCreateDto(this PaymentRequest createPaymentDto)
        {
            return new Donation
            {
                Amount = createPaymentDto.Amount,
                CampaignId = createPaymentDto.CampaignId,
                DonorId = createPaymentDto?.DonorId,
                DonorName = createPaymentDto?.DonorName,
                DonorEmail = createPaymentDto?.DonorEmail,
            };
        }
    }
}
