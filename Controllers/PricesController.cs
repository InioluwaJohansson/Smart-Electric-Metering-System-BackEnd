using Microsoft.AspNetCore.Mvc;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Services;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Smart_Metering_System_BackEnd.Controllers
{
    [Route("SEMS/[controller]")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        IPricesService _pricesService;
        PricesController(IPricesService pricesService)
        {
            _pricesService = pricesService;
        }
        [HttpPut("UpdatePrices")]
        public async Task<IActionResult> UpdatePrices([FromBody] UpdatePricesDto updatePricesDto)
        {
            var prices = await _pricesService.UpdatePrices(updatePricesDto);
            if (prices.Status == true)
            {
                return Ok(prices);
            }
            return Ok(prices);
        }
        [HttpGet("GetPrices")]
        public async Task<IActionResult> GetPrices()
        {
            var prices = await _pricesService.GetPrices();
            if (prices.Status == true)
            {
                return Ok(prices);
            }
            return Ok(prices);
        }
    }
}
