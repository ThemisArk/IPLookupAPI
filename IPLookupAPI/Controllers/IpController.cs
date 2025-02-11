using IPLookupAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IPLookupAPI.Controllers
{
    [Route("api/ip")]
    [ApiController]
    public class IpController : ControllerBase
    {
        public readonly IIpLookupService _iIpLookupservice;

        public IpController(IIpLookupService ipLookupService)
        {
            _iIpLookupservice = ipLookupService;
        }

        [HttpGet("{ip}")]
        public async Task<IActionResult> GetIpDetails(string ip)
        {
            var ipInfo = await _iIpLookupservice.GetIpInformation(ip);
            if(ipInfo == null)
            {
                return NotFound("IP details not found");
            }

            return Ok(ipInfo);
        }
    }
}
