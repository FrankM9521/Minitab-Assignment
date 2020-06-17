using Minitab.Assignment.DomainModels;
using System.Threading.Tasks;

namespace Minitab.Assignment.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<bool> UpsertCustomerAsync(CustomerDomainModel customer);
        Task<CustomerDomainModel> GetByEmailAsync(string emailAddress);
        Task Clear();
    }
}
