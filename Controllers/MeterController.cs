using Microsoft.AspNetCore.Mvc;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Services;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Smart_Metering_System_BackEnd.Controllers
{
    [Route("SEMS/[controller]")]
    [ApiController]
    public class MeterController : ControllerBase
    {
        IMeterService _meterService;
        MeterController(IMeterService meterService)
        {
            _meterService = meterService;
        }
        [HttpPost("CreateMeter")]
        public async Task<IActionResult> CreateMeter([FromBody] CreateMeterDto createMeterDto)
        {
            var meter = await _meterService.CreateMeter(createMeterDto);
            if (meter.Status == true)
            {
                return Ok(meter);
            }
            return Ok(meter);
        }
        // GET: api/<MeterController>
        [HttpPost("AttachMeterToCustomer")]
        public async Task<IActionResult> AttachMeterToCustomer([FromBody] AttachMeterDto attachMeterDto)
        {
            var meter = await _meterService.AttachMeterToCustomer(attachMeterDto);
            if (meter.Status == true)
            {
                return Ok(meter);
            }
            return Ok(meter);
        }
        [HttpPost("AllocateMeterUnit")]
        public async Task<IActionResult> AllocateMeterUnit([FromBody] CreateMeterUnitAllocationDto createMeterUnitAllocationDto)
        {
            var meter = await _meterService.AllocateMeterUnit(createMeterUnitAllocationDto);
            if (meter.Status == true)
            {
                return Ok(meter);
            }
            return Ok(meter);
        }
        [HttpGet("GetMeterById{id}")]
        public async Task<IActionResult> GetMeterById(int id)
        {
            var meter = await _meterService.GetMeterById(id);
            if (meter.Status == true)
            {
                return Ok(meter);
            }
            return Ok(meter);
        }
        [HttpGet("GetAllMeters")]
        public async Task<IActionResult> GetAllMeters()
        {
            var meter = await _meterService.GetAllMeters();
            if (meter.Status == true)
            {
                return Ok(meter);
            }
            return Ok(meter);
        }
    }
}
