using Charity.Dtos.Donation;
using Charity.Dtos.Payment;
using Charity.Helper;
using Charity.Mapper;
using Charity.Models;
using Charity.Repository.IRepository;
using Charity.Service.IService;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Charity.Service
{
    public class DonationSevice : IDonationServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public DonationSevice(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DonationDto> CreateAsync(PaymentRequest paymentRequest)
        {
            Donation donation = paymentRequest.ToPaymentFromCreateDto();
            await _unitOfWork.Donation.AddAsync(donation);
            await _unitOfWork.SaveAsync();
            return donation.ToDonationDto();

        }

        public async Task<QueryObject<DonationDto>> getByIDCampaignAsync(Guid id, int page, int limit)
        {
            var donations =await _unitOfWork.Donation.GetAllAsync(c=>c.CampaignId== id, includeProperties: "Campaign,Donor");
            var donationDto = donations.Select(donation => donation.ToDonationDto()).FilterPage(page, limit);
            return donationDto;
        }
    }
}
