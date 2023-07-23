using Microsoft.AspNetCore.Mvc;
using XmPriceAgg.API.Filters;
using XmPriceAgg.API.Helpers;
using XmPriceAgg.API.Models;
using XmPriceAgg.BLL.Interfaces;

namespace XmPriceAgg.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly ILogger<PriceController> _logger;
        private readonly IAggregationService _aggregationService;

        public PriceController(ILogger<PriceController> logger, IAggregationService aggregationService)
        {
            _logger = logger;   
            _aggregationService = aggregationService;
        }

        [HttpGet("aggPrice")]
        [TimestampConversionFilter]
        [Produces("application/json")]
        public async Task<IActionResult> GetAggPrice([FromQuery] long timestamp)
        {
            _logger.LogInformation("Processing aggPrice request, timestamp: {timestamp}", timestamp);

            var result = await _aggregationService.RetrievePriceAsync(timestamp);
            return Ok(ApiResponse.EnsureSuccess(result));
        }

        [HttpGet("aggPriceList")]
        [TimestampConversionFilter]
        [Produces("application/json")]
        public async Task<IActionResult> GetAggPrice([FromQuery] long startStamp, [FromQuery] long endStamp)
        {
            _logger.LogInformation("Processing aggPriceList request, startStamp: {startStamp}, endStamp: {endStamp}", startStamp, endStamp);

            var result = await _aggregationService.RetrievePricesAsync(startStamp, endStamp);

            return Ok(ApiResponse.EnsureSuccess(result));
        }
    }
}
