using Microsoft.Extensions.Options;
using Minitab.Assignment.Common.Models;
using Minitab.Assignment.Common.Utility;
using Minitab.Assignment.DomainModels;
using Minitab.Assignment.Services.Interfaces;
using Minitab.Assignment.Services.Mappers;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Minitab.Assignment.Services
{
    /// <summary>
    /// Validates an address using the USPS web service
    /// All USPS logic and references are internal to this implementation
    /// to make it easier to change validation services
    /// </summary>
    public class UspsAddressValidationService : IAddressValidationService
    {
        private readonly IOptions<UspsSettings> _options;
        public UspsAddressValidationService(IOptions<UspsSettings> options)
        {
            _options = options;
        }
        public string CreateRequest(AddressDomainModel addressDomainModel)
        {
            var addressXml = GetAddressAsXml(addressDomainModel);
            return BuildAddressValidationUrl(addressXml);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public bool IsValidAddress(Stream stream)
        {
            XDocument doc = XDocument.Load(stream);
            return !doc.Root.Elements().Descendants("Error").Any();
        }

        /// <summary>
        /// Converts an address domain model to an XML string
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private string GetAddressAsXml(AddressDomainModel addressDomainModel)
        {
            var addressValidateRequest = addressDomainModel.ToAddressValidateRequest(_options.Value.UserName);

            return XmlSerializationUtility.GetXmlString(addressValidateRequest);
        }

        /// <summary>
        /// Creates the full Url with parameters
        /// </summary>
        /// <param name="addressXml"></param>
        /// <returns></returns>
        private string BuildAddressValidationUrl(string addressXml)
        {
            return $"{_options.Value.Url}?API=verify&XML={addressXml}";
        }
    }
}
