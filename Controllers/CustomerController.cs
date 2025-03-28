using Microsoft.AspNetCore.Mvc;
using Smart_Electric_Metering_System_BackEnd.Interfaces.Services;
using Smart_Electric_Metering_System_BackEnd.Models.DTOs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Smart_Metering_System_BackEnd.Controllers
{
    [Route("SMSB/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        ICustomerService _customerService;
        CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpPost("CreateCustomer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto createCustomerDto)
        {
            var customer = await _customerService.CreateCustomer(createCustomerDto);
            if (customer.Status == true)
            {
                return Ok(customer);
            }
            return Ok(customer);
        }
        [HttpPut("UpdateCustomer")]
        public async Task<IActionResult> UpdateCustomer([FromBody] UpdateCustomerDto updateCustomerDto)
        {
            var customer = await _customerService.UpdateCustomer(updateCustomerDto);
            if (customer.Status == true)
            {
                return Ok(customer);
            }
            return Ok(customer);
        }
        // GET: api/<CustomerController>
        [HttpGet("GetCustomerById{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await _customerService.GetCustomerById(id);
            if (customer.Status == true)
            {
                return Ok(customer);
            }
            return Ok(customer);
        }
        [HttpGet("GetAllCustomers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customer = await _customerService.GetAllCustomers();
            if (customer.Status == true)
            {
                return Ok(customer);
            }
            return Ok(customer);
        }
    }
}
