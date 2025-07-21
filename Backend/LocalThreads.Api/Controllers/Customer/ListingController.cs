using LocalThreads.Api.DTOs.Request.Customer;
using LocalThreads.Api.Services.Interfaces.Customer;
using Microsoft.AspNetCore.Mvc;

namespace LocalThreads.Api.Controllers.Customer
{
    [ApiController]
    [Route("api/customer/listings")]
    public class ListingController : ControllerBase
    {
        private readonly IListingService _listingService;

        public ListingController(IListingService listingService)
        {
            _listingService = listingService;
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetFilteredProducts([FromBody] ProductFilterRequest request)
        {
            var result = await _listingService.GetFilteredProductsAsync(request);
            return Ok(result);
        }
    }
}
