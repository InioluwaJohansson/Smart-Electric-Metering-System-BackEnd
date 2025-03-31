using Microsoft.AspNetCore.Mvc;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Services;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Smart_Metering_System_BackEnd.Controllers
{
    [Route("SEMS/[controller]")]
    [ApiController]
    public class MeterUnitAllocationController : ControllerBase
    {
        IMeterUnitAllocationService _meterunitallocationService;
        public MeterUnitAllocationController(IMeterUnitAllocationService meterunitallocationService)
        {
            _meterunitallocationService = meterunitallocationService;
        }
        [HttpPost("CreateMeterUnitAllocation")]
        public async Task<IActionResult> CreateMeterUnitAllocation([FromForm] int id, double amount)
        {
            var meterunitallocation = await _meterunitallocationService.CreateUnitAllocation(id, amount);
            if (meterunitallocation.Status == true)
            {
                return Ok(meterunitallocation);
            }
            return Ok(meterunitallocation);
        }
        // GET: api/<MeterUnitAllocationController>
        [HttpGet("GetMeterUnitAllocationById{id}")]
        public async Task<IActionResult> GetMeterUnitAllocationById([FromRoute]int meterId)
        {
            var meterunitallocation = await _meterunitallocationService.GetMeterUnitsAllocation(meterId);
            if (meterunitallocation.Status == true)
            {
                return Ok(meterunitallocation);
            }
            return Ok(meterunitallocation);
        }
    }
}
