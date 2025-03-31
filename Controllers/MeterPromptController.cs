using Microsoft.AspNetCore.Mvc;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Services;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Smart_Metering_System_BackEnd.Controllers
{
    [Route("SEMS/[controller]")]
    [ApiController]
    public class MeterPromptController : ControllerBase
    {
        IMeterPromptService _meterpromptService;
        public MeterPromptController(IMeterPromptService meterpromptService)
        {
            _meterpromptService = meterpromptService;
        }
        [HttpPut("UpdateMeterPrompts{meterId}")]
        public async Task<IActionResult> UpdateMeterPrompts([FromRoute]int meterId)
        {
            var meterprompt = await _meterpromptService.UpdateMeterPrompts(meterId);
            if (meterprompt.Status == true)
            {
                return Ok(meterprompt);
            }
            return Ok(meterprompt);
        }
        // GET: api/<MeterPromptController>
        [HttpGet("GetMeterPrompts{meterId}")]
        public async Task<IActionResult> GetMeterPrompts([FromRoute]int meterId)
        {
            var meterprompt = await _meterpromptService.GetMeterPrompts(meterId);
            if (meterprompt.Status == true)
            {
                return Ok(meterprompt);
            }
            return Ok(meterprompt);
        }
    }
}
