using Minitab.Assignment.DomainModels;
using Minitab.Assignment.Services.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace Minitab.Assignment.Services
{
    /// <summary>
    /// Address service
    /// The only method this service has is validate address async
    /// I extracted any reference to the specific type of address validation being used
    /// to make switching address validation services easier. The address validation is
    /// injected as a dependency
    /// </summary>
    public class AddressService : IAddressService
    {
        private readonly IAddressValidationService _addressValidator;
        private readonly IHttpClientFactory _httpClientFactory;
        public AddressService(IAddressValidationService addressValidator, IHttpClientFactory httpClientFactory)
        {
            _addressValidator = addressValidator;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Validates the supplied address with the postal service validator
        /// </summary>
        /// <param name="addressDomainModel">The address to validate against the address validation service</param>
        /// <returns></returns>
        public async Task<bool> ValidAddressAsync(AddressDomainModel addressDomainModel)
        {
            HttpResponseMessage response;

            // Disposing of HttpClient is not needed, it is managed by the IHttpClientFactory
            var client = _httpClientFactory.CreateClient();
            response = await client.GetAsync(_addressValidator.CreateRequest(addressDomainModel));

            using var stream = await response.Content.ReadAsStreamAsync();
                return _addressValidator.IsValidAddress(stream);
        }
    }
}
