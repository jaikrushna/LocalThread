using LocalThreads.Api.DTOs.Request.Customer;
using LocalThreads.Services.Interfaces.Customer;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LocalThreads.Api.Controllers.Customer
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCustomerDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName) || string.IsNullOrWhiteSpace(dto.PhoneNumber))
                return BadRequest("FullName and PhoneNumber are required");

            var customerId = await _customerService.RegisterCustomerAsync(dto);
            return Ok(new { CustomerId = customerId });
        }

        // ✅ Fetch user by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(string id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null) return NotFound("Customer not found");

            return Ok(customer);
        }

        // ✅ Update user
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(string id, [FromBody] UpdateCustomerDto dto)
        {
            await _customerService.UpdateCustomerAsync(id, dto);
            return Ok("Customer updated successfully");
        }
    }
}
