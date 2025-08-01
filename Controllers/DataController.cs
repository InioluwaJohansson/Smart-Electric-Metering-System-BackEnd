using Microsoft.AspNetCore.Mvc;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Services;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Smart_Metering_System_BackEnd.Controllers
{
    [Route("SEMS/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        IDataService _dataService;
        public DataController(IDataService dataService)
        {
            _dataService = dataService;
        }
        [HttpGet("EstablishConnectionByESP32")]
        public async Task<IActionResult> EstablishConnectionByESP32(string MeterId, string auth)
        {
            var data = await _dataService.EstablishConnection(MeterId, auth);
            if (data.Status == true)
            {
                return Ok(data);
            }
            return Ok(data);
        }
        // GET: api/<DataController>
        [HttpGet("MeterUnitsData")]
        public async Task<IActionResult> MeterUnitsData(int Meterid)
        {
            var data = await _dataService.MeterUnitsData(Meterid);
            if (data.Status == true)
            {
                return Ok(data);
            }
            return Ok(data);
        }
        [HttpPost("MeterUnitsDataFromESP32")]
        public async Task<IActionResult> MeterUnitsDataFromESP32(CreateMeterUnitsDto createMeterUnitsDto)
        {
            var data = await _dataService.MeterUnitsDataFromESP32(createMeterUnitsDto);
            if (data.Status == true)
            {
                return Ok(data);
            }
            return Ok(data);
        }
        [HttpGet("GetDashBoardData")]
        public async Task<IActionResult> GetDashBoardData()
        {
            var data = await _dataService.GetDashBoardData();
            if (data.Status == true)
            {
                return Ok(data);
            }
            return Ok(data);
        }
        [HttpGet("GetDashBoardTransactionData")]
        public async Task<IActionResult> GetDashBoardTransactionData()
        {
            var data = await _dataService.GetAllTransactions();
            if (data.Status == true)
            {
                return Ok(data);
            }
            return Ok(data);
        }
    }
}
