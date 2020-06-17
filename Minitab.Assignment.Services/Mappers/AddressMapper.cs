using Minitab.Assignment.DomainModels;
using Minitab.Assignment.Services.Models.Usps;

namespace Minitab.Assignment.Services.Mappers
{
    public static class AddressMapper
    {
        /// <summary>
        /// Maps the domain address model to USPS format
        /// </summary>
        /// <param name="address"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static AddressValidateRequest ToAddressValidateRequest(this AddressDomainModel address, string userId)
        {
            return new AddressValidateRequest
            {
                Address = new AddressXML
                {
                    Address1 = address.Line1,
                    Address2 = "",
                    City = address.City,
                    Id = "0",
                    State = address.State,
                    Zip4 = "",
                    Zip5 = address.PostalCode
                },
                UserId = userId
            };
        }

        /// <summary>
        /// Merges a valid USPS address with an address domain model
        /// </summary>
        /// <param name="addressDomain"></param>
        /// <param name="addressValidateResponse"></param>
        /// <returns></returns>
        public static AddressDomainModel ToCleanAddress(this  AddressDomainModel addressDomainModel, AddressXML addressXML)
        {
            addressDomainModel.City = addressXML.City;
            addressDomainModel.Line1 = addressXML.Address1;
            addressDomainModel.PostalCode = $"{addressXML.Zip5}-{addressXML.Zip4}";
            addressDomainModel.State = addressXML.State;
            return addressDomainModel;
        }
    }
}
