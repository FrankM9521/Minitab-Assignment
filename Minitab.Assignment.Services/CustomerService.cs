using Minitab.Assignment.CrmStub.Interfaces;
using Minitab.Assignment.DomainModels;
using Minitab.Assignment.Services.Interfaces;
using System.Threading.Tasks;

namespace Minitab.Assignment.Services
{
    /// <summary>
    /// Upserts a customer to a mock CRM
    /// </summary>
    public class CustomerService  : ICustomerService
    {
        private readonly IAddressService _addressService;
        private readonly ICrmRepository _crmRepository;
        public CustomerService(IAddressService addressService, ICrmRepository crmRepository)
        {
            _addressService = addressService;
            _crmRepository = crmRepository;
        }

        /// <summary>
        /// Inserts a customer into the CRM. If the customer's address is valid, their address is also inserted.
        /// Otherwise only the customer is inserted
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<bool> UpsertCustomerAsync(CustomerDomainModel customer)
        {
            var validAddress = await _addressService.ValidAddressAsync(customer.Address);
            
            if (!validAddress)
            {
                customer.Address = null;
            }

            await _crmRepository.UpsertCustomer(customer);
            return validAddress;
        }

        /// <summary>
        /// For e2e testing
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
       public async Task<CustomerDomainModel> GetByEmailAsync(string emailAddress)
        {
            return await _crmRepository.GetByEmail(emailAddress);
        }

        /// <summary>
        /// For e2e testing
        /// </summary>
        /// <returns></returns>
        public async Task Clear()
        {
            await _crmRepository.Clear();
        }
    }
}
