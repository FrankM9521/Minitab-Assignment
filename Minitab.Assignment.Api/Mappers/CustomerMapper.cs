using Minitab.Assignment.DataContracts;
using Minitab.Assignment.DomainModels;

namespace Minitab.Assignment.Api.Mappers
{
    public static class CustomerMapper
    {
        public static CustomerDomainModel ToCustomerDomainModel(this Customer customer)
        {
            return new CustomerDomainModel
            {
                Address = new AddressDomainModel
                {
                    City = customer.Address.City,
                    Country = customer.Address.Country,
                    Line1 = customer.Address.Line1,
                    PostalCode = customer.Address.PostalCode,
                    State = customer.Address.State
                },
                CustomerEmail = customer.CustomerEmail,
                CustomerName = customer.CustomerName
            };
        }
    }
}
