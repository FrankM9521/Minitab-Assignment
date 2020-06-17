using System.Xml.Serialization;

namespace Minitab.Assignment.Services.Models.Usps
{
    [XmlRoot("AddressValidateRequest")]
    public class AddressValidateRequest
    {
        [XmlAttribute(AttributeName = "USERID")]
        public string UserId { get; set; }
        [XmlElement(ElementName = "Address")]
        public AddressXML Address { get; set; }
    }
}
