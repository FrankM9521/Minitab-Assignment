using Microsoft.AspNetCore.Mvc.Testing;
using Minitab.Assignment.DataContracts;
using Minitab.Assignment.DomainModels;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Minitab.Assignment.Api.Tests
{
    [Collection("Automated Tests")]
    public class CustomerControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;
        public CustomerControllerTests(
                    CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //     Automated Test
        //     3 tests are required, but there are only 3 conditions. There are other integration tests in Minitab.Assignment.Services.Tests
        //     For full testing, since error conditions are being ignored as per the requirements doc, there are only 2 conditions to test
        //     Those are valid address and invalid address. For the 3rs, I will insert a valid and an invalid at the same time
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// TEST ONE
        /// When a customer with a valid email address is passed in, the customer AND email address
        /// are added to the database
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task WhenACustomerWithAValidAddressIsAdded_ShouldInsertTheCustomerAndAddress()
        {
            // Deletes from customer and address tables
            await AssertDatabaseIsClear();

            //Create customer
            var customer = CreateFredFlinstoneValidAddress();

            // Create customer post
            await PostCustomer(customer);

            //Retrieve customer from database to validate against the posted customer
            var newCustomer = await GetCustomer(customer.CustomerEmail);

            //Assert customer has valid address
            AssertValidCustomerAndAddress(customer, newCustomer);
        }

        [Fact]
        public async Task WhenACustomerWithnInValidAddressIsAdded_ShouldOnlyInsertTheCustomer()
        {
            // Deletes from customer and address tables
            await AssertDatabaseIsClear();

            //Create customer
            var customer = CreateBarneyRubbleInValidAddress();

            // Create customer post
            await PostCustomer(customer);

            //Retrieve customer from database to validate against the posted customer
            var newCustomer = await GetCustomer(customer.CustomerEmail);

            //Assert customer has valid address
            AssertValidCustomerWithInValidAddress(customer, newCustomer);
        }

        [Fact]
        public async Task WhenOneCustomerHasInvalidAddressAndAnotherHasValid_ShouldOnlyInsertTheValidCustomersAddress()
        {
            // Deletes from customer and address tables
            await AssertDatabaseIsClear();

            //Create customer
            var invalidCustomer = CreateBarneyRubbleInValidAddress();
            var validCustomer = CreateFredFlinstoneValidAddress();
            // Create customer post
            await PostCustomer(invalidCustomer);
            await PostCustomer(validCustomer);

            //Retrieve customer from database to validate against the posted customer
            var invalidNewCustomer = await GetCustomer(invalidCustomer.CustomerEmail);
            var validNewCustomer = await GetCustomer(validCustomer.CustomerEmail);

            //Assert customer has valid address
            AssertValidCustomerWithInValidAddress(invalidCustomer, invalidNewCustomer);
            AssertValidCustomerAndAddress(validCustomer, validNewCustomer);
        }

        private void AssertValidCustomerWithInValidAddress(Customer customer, CustomerDomainModel newCustomer)
        {
            Assert.Equal(customer.CustomerEmail, newCustomer.CustomerEmail);
            Assert.Equal(customer.CustomerName, newCustomer.CustomerName);
            Assert.Null(newCustomer.Address);
        }

        private void AssertValidCustomerAndAddress(Customer customer, CustomerDomainModel newCustomer)
        {
            Assert.Equal(customer.CustomerEmail, newCustomer.CustomerEmail);
            Assert.Equal(customer.CustomerName, newCustomer.CustomerName);
            Assert.Equal(customer.Address.City, newCustomer.Address.City);
            Assert.Equal(customer.Address.Country, newCustomer.Address.Country);
            Assert.Equal(customer.Address.Line1, newCustomer.Address.Line1);
            Assert.Equal(customer.Address.PostalCode, newCustomer.Address.PostalCode);
            Assert.Equal(customer.Address.State, newCustomer.Address.State);
        }

        private async Task<CustomerDomainModel> GetCustomer(string emailAddress)
        {
            var getResponse = await _client.GetAsync($"/api/customer?emailAddress={emailAddress}");
            var newCustomer = JsonConvert.DeserializeObject<CustomerDomainModel>(await getResponse.Content.ReadAsStringAsync());

            Assert.Equal(System.Net.HttpStatusCode.OK, getResponse.StatusCode);
            return newCustomer;
        }

        private async Task PostCustomer(Customer customer)
        {
            var json = JsonConvert.SerializeObject(customer);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            httpContent.Headers.ContentType.MediaType = MediaTypeNames.Application.Json;
            var response = await _client.PostAsync($"/api/customer", httpContent);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Deletes Customer and address for a fresh test run
        /// </summary>
        private async Task AssertDatabaseIsClear()
        {
            var clearResponse = await _client.GetAsync("/api/customer/clear");
            Assert.Equal(System.Net.HttpStatusCode.OK, clearResponse.StatusCode);
        }

        private Customer CreateBarneyRubbleInValidAddress()
        {
            return new Customer
            {
                Address = new Address
                {
                    City = "Bedrock",
                    Country = "US",
                    Line1 = "12 Dino Road",
                    PostalCode = "01010",
                    State = "NE"
                },
                CustomerEmail = "barney.rubble@bedrock-llc.com",
                CustomerName = "Barney Rubble"
            };
        }

        private Customer CreateFredFlinstoneValidAddress()
        {
            return new Customer
            {
                Address = new Address
                {
                    City = "Chicago",
                    Country = "US",
                    Line1 = "10 S LaSalle St.",
                    PostalCode = "60603",
                    State = "IL"
                },
                CustomerEmail = "fred.flinstone@bedrock-llc.com",
                CustomerName = "Fred Filnstone"
            };
        }
    }
}
