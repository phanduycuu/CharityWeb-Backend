
using Charity.Dtos.Campaign;
using Charity.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Charity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampaignController : Controller
    {
        private readonly ICampaignServices _campaignService;
        public CampaignController(ICampaignServices campaignService)
        {
            _campaignService = campaignService;
        }

        [HttpGet("getByID/{id:Guid}")]
        public async Task<IActionResult> GetByID([FromRoute] Guid id)
        {
            CampaignDto? campaignExisting = await _campaignService.getByIDAsync(id);
            if (campaignExisting == null) return NotFound();
            return Ok(campaignExisting);
        }

        [HttpPost("create")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] CreateCampaign campaignCreateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var supplierRS = await _campaignService.CreateAsync(campaignCreateDto);
            return Ok(supplierRS);
        }

        //[HttpPut("toggleStatus/{id:int}")]
        //public async Task<IActionResult> UpdateStatus([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);
        //    var campaignUpdated = await _campaignService.UpdateStatusAsync(id);
        //    if (campaignUpdated == null) return NotFound();
        //    return Ok(campaignUpdated);
        //}

        //[HttpPut("update/{id:int}")]
        //public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatecampaignDto campaignUpdateDto)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);
        //    var campaignRS = await _campaignService.UpdateAsync(id, campaignUpdateDto);
        //    if (campaignRS == null) return NotFound();
        //    return Ok(campaignRS);
        //}

        [HttpGet("getCampaign")]
        public async Task<IActionResult> getCampaign([FromQuery] int page = 1, [FromQuery] int limit = 12)
        {
            var campaigs = await _campaignService.GetCampaignsAsync(page, limit);
            return Ok(campaigs);
        }
        [HttpPost("filter/")]
        public async Task<IActionResult> SearchAdvance([FromBody] FilterCampaignUserRequest filterProRequest)
        {
            var result = await _campaignService.searchByKeyAsync(filterProRequest);
            return Ok(result);
        }
        [HttpGet("getStatisticalUser")]
        public async Task<IActionResult> getStatisticalUser()
        {
            var statisticalUser = await _campaignService.getStatisticalUser();
            return Ok(statisticalUser);
        }

    }
}
