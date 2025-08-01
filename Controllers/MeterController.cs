﻿using Microsoft.AspNetCore.Mvc;
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
        public MeterController(IMeterService meterService)
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
        [HttpPut("UpdateMeter")]
        public async Task<IActionResult> UpdateMeter([FromBody] UpdateMeterDto updateMeterDto)
        {
            var meter = await _meterService.UpdateMeter(updateMeterDto);
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
        [HttpGet("UpdateMeterStatus{id}")]
        public async Task<IActionResult> UpdateMeterStatus([FromRoute]int id)
        {
            var meter = await _meterService.UpdateMeterStatus(id);
            if (meter.Status == true)
            {
                return Ok(meter);
            }
            return Ok(meter);
        }
        [HttpGet("GetMeterById{id}")]
        public async Task<IActionResult> GetMeterById([FromRoute]int id)
        {
            var meter = await _meterService.GetMeterById(id);
            if (meter.Status == true)
            {
                return Ok(meter);
            }
            return Ok(meter);
        }
        [HttpGet("GetMetersByUserId{id}")]
        public async Task<IActionResult> GetMeterByUserId([FromRoute]int id)
        {
            var meter = await _meterService.GetMeterByUserId(id);
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
        [HttpGet("GetAllMeterUnits")]
        public async Task<IActionResult> GetAllMeterUnits()
        {
            var meter = await _meterService.GetAllMeterUnits();
            if (meter.Status == true)
            {
                return Ok(meter);
            }
            return Ok(meter);
        }
    }
}
