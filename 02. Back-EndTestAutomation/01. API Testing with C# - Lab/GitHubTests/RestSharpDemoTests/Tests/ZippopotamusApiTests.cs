using Newtonsoft.Json;
using NUnit.Framework.Legacy;
using RestSharp;
using RestSharpDemoTests.Models;

namespace RestSharpDemoTests.Tests
{
    public class ZippopotamusApiTests
    {
        [TestCase("BG", "1000", "Sofija")]
        [TestCase("BG", "5000", "Veliko Turnovo")]
        [TestCase("CA", "M5S", "Toronto")]
        [TestCase("GB", "B1", "Birmingham")]
        [TestCase("DE", "01067", "Dresden")]
        public void TestZipopotamus(string countryCode, string zipCode, string expectedPlace)
        {
            // Arrange
            var restClient = new RestClient("https://api.zippopotam.us");
            var httpRequest = new RestRequest($"{countryCode}/{zipCode}");

            // Act
            var httpRsponse = restClient.Execute(httpRequest);
            var location = JsonConvert.DeserializeObject<Location>(httpRsponse.Content);

            // Assert
            StringAssert.Contains(expectedPlace, location.Places[0].PlaceName);
        }
    }
}
