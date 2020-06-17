using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minitab.Assignment.Api.Mappers;
using Minitab.Assignment.DataContracts;
using Minitab.Assignment.Services.Interfaces;
using System.Threading.Tasks;

namespace Minitab.Assignment.Api.Controllers
{
    /// <summary>
    /// Customer Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        
        public CustomerController(ICustomerService customerSerevice)
        {
            _customerService = customerSerevice;
        }

        /// <summary>
        /// Posts a customer and address. If the address in cannot be validated by the postal validator,
        /// only the customer will be inserted. Otherwise both the customer and the address will be inserted
        /// into the mock CRM system
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Post(Customer customer)
        {
            await _customerService.UpsertCustomerAsync(customer.ToCustomerDomainModel());

            return Ok();
        }

        /// <summary>
        /// Used for integration testing to validate that the user was inserted. Returns a domain model
        /// since this is just a test stub
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get(string emailAddress)
        {
            var result = await _customerService.GetByEmailAsync(emailAddress);

            return Ok(result);
        }

        /// <summary>
        /// Used for integration testing to delete all records from the Customer and Address tables
        /// before each test run.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Clear")]
        public async Task<IActionResult> Clear()
        {
            await _customerService.Clear();

            return Ok();
        }
    }
}
