using System;
using System.Collections.Generic;

namespace LocalThreads.Api.DTOs.Request.Shopkeeper;

    public class OrderRequestDto
    {
        public string CustomerId { get; set; }
        public string ShopkeeperId { get; set; }
        public List<OrderItemDto> Items { get; set; }
        public string CustomerAddress { get; set; }
        public DateTime RequestedDeliveryTime { get; set; }
    }

    public class OrderItemDto
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }

