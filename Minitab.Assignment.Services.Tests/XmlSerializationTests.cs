using Minitab.Assignment.Common.Utility;
using Minitab.Assignment.Services.Models.Usps;
using Xunit;

namespace Minitab.Assignment.Services.Tests
{
    public class XmlSerializationTests
    {
        [Fact]
        public void WhenXMLIsSerialized_ShouldMatchUspsSchema()
        {
            var expectedXML = "<AddressValidateRequest USERID=\"xxx\"><Address ID=\"0\"><Address1>123 Main St</Address1>" +
                "<Address2 /><City>Bedrock</City><State>IL</State><Zip5>90210</Zip5><Zip4 /></Address></AddressValidateRequest>";  

            var address = new AddressValidateRequest
            {
                Address = new AddressXML
                {
                    Address1 = "123 Main St",
                    Address2 = "",
                    City = "Bedrock",
                    Id = "0",
                    State = "IL",
                    Zip4 = "",
                    Zip5 = "90210"
                },
                UserId = "xxx"
            };
            var xml = XmlSerializationUtility.GetXmlString<AddressValidateRequest>(address);
            Assert.Equal(expectedXML, xml);
        }
    }
}
