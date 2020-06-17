using System.Xml.Serialization;

namespace Minitab.Assignment.Services.Models.Usps
{
    [XmlRoot("AddressValidateResponse")]
    public class AddressValidateResponse
    {
        public AddressXML Address { get; set; }
    }
}
