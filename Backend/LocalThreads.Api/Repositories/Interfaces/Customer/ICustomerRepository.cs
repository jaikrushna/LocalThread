using LocalThreads.Models.Customer;
using System.Threading.Tasks;

namespace LocalThreads.Repositories.Interfaces.Customer
{
    public interface ICustomerRepository
    {
        Task<string> CreateAsync(CustomerRegistration customer);
        Task<CustomerRegistration> GetByPhoneAsync(string phoneNumber);
        Task<CustomerRegistration> GetByIdAsync(string customerId);
        Task UpdateAsync(CustomerRegistration customer);
    }
}
