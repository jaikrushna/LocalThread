using System;
using System.Collections.Generic;

namespace LocalThreads.Api.DTOs.Response.Shopkeeper;

    public class OrderResponseDto
    {
        public string Id { get; set; }
        public string CustomerName { get; set; }     // Optional for UI
        public string CustomerContact { get; set; }  // Optional
        public List<OrderItemDetailsDto> Items { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Shipping { get; set; } = 0;   // Assuming free
        public decimal Total => Subtotal + Shipping;
        public string CustomerAddress { get; set; }
        public string Status { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? RequestedDeliveryTime { get; set; }
    }

    public class OrderItemDetailsDto
    {
        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

