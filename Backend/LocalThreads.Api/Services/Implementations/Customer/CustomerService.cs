using LocalThreads.Api.DTOs.Request.Customer;
using LocalThreads.Models;
using LocalThreads.Models.Customer;
using LocalThreads.Repositories.Interfaces;
using LocalThreads.Repositories.Interfaces.Customer;
using LocalThreads.Services.Interfaces;
using LocalThreads.Services.Interfaces.Customer;
using System;
using System.Threading.Tasks;

namespace LocalThreads.Services.Implementations.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<string> RegisterCustomerAsync(RegisterCustomerDto dto)
        {
            var customer = new CustomerRegistration
            {
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                CreatedAt = DateTime.UtcNow
            };

            return await _customerRepository.CreateAsync(customer);
        }

        public async Task UpdateCustomerAsync(string id, UpdateCustomerDto dto)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) return;

            customer.FullName = dto.FullName;
            customer.Email = dto.Email;
            customer.Gender = dto.Gender;
            customer.AlternateMobile = dto.AlternateMobile;
            customer.Location = dto.Location;
            customer.Dob = dto.Dob?.Date;
            customer.HintName = dto.HintName;

            await _customerRepository.UpdateAsync(customer);
        }

        public async Task<CustomerRegistration> GetCustomerByIdAsync(string id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

    }
}
