namespace LocalThreads.Api.DTOs.Request.Customer
{
    public class UpdateCustomerDto
    {
        public string FullName { get; set; }

        public string Email { get; set; }
        public string Gender { get; set; }
        public string AlternateMobile { get; set; }
        public string Location { get; set; }

        public DateTime? Dob { get; set; }
        public string HintName { get; set; }
    }
}
