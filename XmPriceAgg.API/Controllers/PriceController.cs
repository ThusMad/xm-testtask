using Microsoft.AspNetCore.Mvc;

namespace XmPriceAgg.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        [HttpGet("aggPrice")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAggPrice([FromQuery] long timestamp)
        {
            throw new NotImplementedException();
        }

        [HttpGet("aggPriceList")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAggPrice([FromQuery] long startStamp, [FromQuery] long endStamp)
        {
            throw new NotImplementedException();
        }
    }
}
