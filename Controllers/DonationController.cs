using Charity.Helper;
using Charity.Service;
using Charity.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Charity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonationController : ControllerBase
    {

        private readonly IDonationServices _donationServices;
        public DonationController( IDonationServices donationServices)
        {
            _donationServices = donationServices;
        }

        [HttpGet("getDonationByidCampaign")]
        public async Task<IActionResult> getCampaign([FromQuery] Guid id,[FromQuery] int page = 1, [FromQuery] int limit = 12)
        {
            var donations = await _donationServices.getByIDCampaignAsync(id,page, limit);
            return Ok(donations);
        }
    }
}
