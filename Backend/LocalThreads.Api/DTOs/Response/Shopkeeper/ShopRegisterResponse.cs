namespace LocalThreads.Api.DTOs.Response.Shopkeeper;

    public class ShopRegisterResponse
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public bool IsPendingApproval { get; set; }= true;
    }

