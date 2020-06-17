using Minitab.Assignment.DomainModels;
using System.IO;

namespace Minitab.Assignment.Services.Interfaces
{
    /// <summary>
    /// Interface for implementation details of an address validation service
    /// </summary>
    public interface IAddressValidationService
    {
        string CreateRequest(AddressDomainModel addressDomainModel);
        bool IsValidAddress(Stream stream);
    }
}
