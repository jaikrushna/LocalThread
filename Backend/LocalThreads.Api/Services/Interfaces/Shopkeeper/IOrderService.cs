using LocalThreads.Api.DTOs.Request.Shopkeeper;
using LocalThreads.Api.DTOs.Response.Shopkeeper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocalThreads.Services.Interfaces.Shopkeeper
{
    public interface IOrderService
    {
        // GET all orders needing approval
        Task<IEnumerable<OrderResponseDto>> GetPendingOrdersAsync(string shopkeeperId);

        // GET all approved but not yet delivered orders
        Task<IEnumerable<OrderResponseDto>> GetApprovedOrdersAsync(string shopkeeperId);

        // GET all delivered/completed orders
        Task<IEnumerable<OrderResponseDto>> GetCompletedOrdersAsync(string shopkeeperId);

        // PUT to update the status of an order (Approve, Reject, Deliver)
        Task<bool> UpdateOrderStatusAsync(string orderId, string newStatus);

        // POST new order from customer
        Task<string> CreateOrderAsync(OrderRequestDto orderRequestDto);
    }
}
