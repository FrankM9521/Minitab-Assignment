using Microsoft.Extensions.Options;
using Minitab.Assignment.Common.Models;
using Minitab.Assignment.Common.Utility;
using Minitab.Assignment.DomainModels;
using Minitab.Assignment.Services.Models.Usps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace Minitab.Assignment.Services.Tests
{
    [Collection("USPS Validation")]
    public class UspsValidationTests
    {
        private readonly IOptions<UspsSettings> _options;
        public UspsValidationTests()
        {
            _options = Options.Create(new UspsSettings
            {
                Password = "420CJ92IJ891",
                Url = "http://production.shippingapis.com/ShippingAPI.dll",
                UserName = "633FMCON0605"
            });
        }

        [Fact]
        public async Task WhenInValidRequestIsMade_ShouldReturnErrorNode()
        {
            Environment.SetEnvironmentVariable("UspsSettings:UserName", "633FMCON0605");
            var address = new AddressValidateRequest
            {
                Address = new AddressXML
                {
                    Address1 = "9625 W 99th Pl",
                    Address2 = "",
                    City = "Oak Lawn",
                    Id = "0",
                    State = "IL",
                    Zip4 = "",
                    Zip5 = "60453"
                },
                UserId = _options.Value.UserName
            };

            var xml = XmlSerializationUtility.GetXmlString(address);
            var url2 = $"https://secure.shippingapis.com/shippingApi.dll?API=verify&XML={xml}";

            var http = new HttpClient();
            var response = await http.GetAsync(url2);
            var stream = await response.Content.ReadAsStreamAsync();

            XDocument doc = XDocument.Load(stream);

            IEnumerable<XElement> nodes =
                doc.Root.Elements().Descendants("Error");

            Assert.Single(nodes);
        }

        [Fact]
        public async Task WhenValidRequestIsMade_ShouldReturnAddress()
        {
            Environment.SetEnvironmentVariable("UspsSettings:UserName", "633FMCON0605");
            var address = new AddressValidateRequest
            {
                Address = new AddressXML
                {
                    Address1 = "5525 W 99th Pl",
                    Address2 = "",
                    City = "Oak Lawn",
                    Id = "0",
                    State = "IL",
                    Zip4 = "",
                    Zip5 = "60453"
                },
                UserId = _options.Value.UserName
            };

            var xml = XmlSerializationUtility.GetXmlString(address);
            var url2 = $"https://secure.shippingapis.com/shippingApi.dll?API=verify&XML={xml}";

            var http = new HttpClient();
            var response = await http.GetAsync(url2);
            var stream = await response.Content.ReadAsStreamAsync();

            XDocument doc = XDocument.Load(stream);

            IEnumerable<XElement> nodes =
                doc.Root.Elements().Descendants("Error");

            Assert.Empty(nodes);
 
            stream.Position = 0;
            var obj = XmlSerializationUtility.Deserialize<AddressValidateResponse>(stream);
            Assert.NotNull(obj);
 
        }
    }
}
