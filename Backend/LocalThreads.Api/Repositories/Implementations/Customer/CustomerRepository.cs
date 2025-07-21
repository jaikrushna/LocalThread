using LocalThreads.Models;
using LocalThreads.Models.Customer;
using LocalThreads.Repositories.Interfaces.Customer;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace LocalThreads.Repositories.Implementations.Customer
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IMongoCollection<CustomerRegistration> _customers;

        public CustomerRepository(IMongoDatabase database)
        {
            _customers = database.GetCollection<CustomerRegistration>("customers");
        }

        public async Task<string> CreateAsync(CustomerRegistration customer)
        {
            await _customers.InsertOneAsync(customer);
            return customer.Id;
        }

        public async Task<CustomerRegistration> GetByPhoneAsync(string phoneNumber)
        {
            return await _customers.Find(c => c.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
        }

        public async Task<CustomerRegistration> GetByIdAsync(string customerId)
        {
            return await _customers.Find(c => c.Id == customerId).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(CustomerRegistration customer)
        {
            var filter = Builders<CustomerRegistration>.Filter.Eq(c => c.Id, customer.Id);
            await _customers.ReplaceOneAsync(filter, customer);
        }
    }
}
