using Charity.Dtos.Payment;
using Charity.Helper;
using Charity.Service;
using Charity.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Charity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly VnpayConfig _vnpayConfig;
        private readonly IDonationServices _donationServices;
        private readonly ICampaignServices _campaignServices;
        public PaymentController(IOptions<VnpayConfig> options, IDonationServices donationServices, ICampaignServices campaignServices)
        {
            _vnpayConfig = options.Value;
            _donationServices = donationServices;
            _campaignServices = campaignServices;
        }

        [HttpPost("create")]
        public IActionResult CreatePayment([FromBody] PaymentRequest payment)
        {
            var vnp = new VnPayLibrary();
            var tick = DateTime.UtcNow.Ticks.ToString();

            vnp.AddRequestData("vnp_Version", "2.1.0");
            vnp.AddRequestData("vnp_Command", "pay");
            vnp.AddRequestData("vnp_TmnCode", _vnpayConfig.TmnCode);
            vnp.AddRequestData("vnp_Amount", ((long)payment.Amount * 100).ToString());
            vnp.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnp.AddRequestData("vnp_CurrCode", "VND");
            vnp.AddRequestData("vnp_IpAddr", HttpContext.Connection.RemoteIpAddress?.ToString());
            vnp.AddRequestData("vnp_Locale", "vn");
            vnp.AddRequestData("vnp_OrderInfo", $"{payment.CampaignId}|{payment.DonorName}|{payment.DonorEmail}|{payment.DonorId}");
            vnp.AddRequestData("vnp_OrderType", "other");
            vnp.AddRequestData("vnp_ReturnUrl", _vnpayConfig.ReturnUrl);
            vnp.AddRequestData("vnp_TxnRef", tick);

            string paymentUrl = vnp.CreateRequestUrl(_vnpayConfig.PaymentUrl, _vnpayConfig.HashSecret);

            return Ok(new { paymentUrl });
        }

        [HttpGet("return")]
        public async Task<IActionResult> PaymentReturn()
        {
            var vnp = new VnPayLibrary();

            foreach (var key in Request.Query.Keys)
            {
                if (key.StartsWith("vnp_"))
                    vnp.AddResponseData(key, Request.Query[key]);
            }

            var isValid = vnp.ValidateSignature(Request.Query["vnp_SecureHash"], _vnpayConfig.HashSecret);

            if (isValid && Request.Query["vnp_ResponseCode"] == "00")
            {
                var orderInfo = Request.Query["vnp_OrderInfo"].ToString();
                var parts = orderInfo.Split('|');
                var CampaignId = parts[0];
                var DonorName = parts[1];
                var DonorEmail = parts[2];
                var DonorId = parts[3];
                var amount = long.Parse(Request.Query["vnp_Amount"]) / 100;
                PaymentRequest paymentRequest = new PaymentRequest();
                paymentRequest.Amount=  amount;
                paymentRequest.DonorName= DonorName;
                paymentRequest.CampaignId = Guid.Parse(CampaignId);
                paymentRequest.DonorEmail= DonorEmail;
                if(DonorId!="" && DonorId !=null)
                    paymentRequest.DonorId= Guid.Parse(DonorId);
                var donation= await _donationServices.CreateAsync(paymentRequest);
                var campaign=await _campaignServices.UpdateCurrentAmountasync(Guid.Parse(CampaignId), amount);
                return Redirect("http://localhost:3000/system");
                //return Ok(campaign);
            }

            return BadRequest("Thanh toán thất bại hoặc sai chữ ký");
        }
    }
}
