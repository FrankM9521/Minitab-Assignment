using Minitab.Assignment.DomainModels;
using System.Threading.Tasks;

namespace Minitab.Assignment.Services.Interfaces
{
    public interface IAddressService
    {
        Task<bool> ValidAddressAsync(AddressDomainModel address);
    }
}
