using System.Threading.Tasks;
using LocalThreads.Api.DTOs.Request.Customer;
using LocalThreads.Models.Customer;

namespace LocalThreads.Services.Interfaces.Customer
{
    public interface ICustomerService
    {
        Task<string> RegisterCustomerAsync(RegisterCustomerDto dto);
        Task UpdateCustomerAsync(string id, UpdateCustomerDto dto);
        Task<CustomerRegistration> GetCustomerByIdAsync(string id);

    }
}
