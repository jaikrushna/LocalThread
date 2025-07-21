namespace LocalThreads.Api.DTOs.Requests.Shopkeeper
{
    public class ShopRegisterRequest
    {
        public string ShopName { get; set; }
        public string OwnerName { get; set; }
        public string PhoneNumber { get; set; }
        public string FirebaseIdToken { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string Category { get; set; }
        public string BusinessDescription { get; set; }

        public IFormFile ShopImageFile { get; set; }
        public IFormFile GovernmentIdFile { get; set; }
    }
}
