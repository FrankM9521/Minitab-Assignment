using Minitab.Assignment.DomainModels;
using System.Threading.Tasks;

namespace Minitab.Assignment.CrmStub.Interfaces
{
    public interface ICrmRepository
    {
        Task UpsertCustomer(CustomerDomainModel customerDomainModel);
        Task<CustomerDomainModel> GetByEmail(string emailAddress);
        Task Clear();
    }
}
