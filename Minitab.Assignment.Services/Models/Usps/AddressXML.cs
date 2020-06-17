using System.Xml.Serialization;

namespace Minitab.Assignment.Services.Models.Usps
{
    public class AddressXML
    {
        [XmlAttribute(AttributeName = "ID")]
        public string Id { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip5 { get; set; }
        public string Zip4 { get; set; }
    }



}
