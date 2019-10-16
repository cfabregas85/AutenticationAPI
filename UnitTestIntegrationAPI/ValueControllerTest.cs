using AutenticationAPI;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace UnitTestIntegrationAPI
{
    [TestClass]
    public class ValueControllerTest
    {
        private WebApplicationFactory<Startup> _factory;

        [TestInitialize]
        public void Initialize()
        {
            _factory = new WebApplicationFactory<Startup>();
        }

        [TestMethod]
        public async Task ActionResultGetValue()
        {
            // Arrange
            var client = _factory.CreateClient();
            var url = "/api/values";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Assert.IsTrue(false, "Unsuccessful request" + response.StatusCode);
            }

            //Act
            var result = JsonConvert.DeserializeObject<string[]>
                        (await response.Content.ReadAsStringAsync());

            //Assert
            Assert.AreEqual(expected: 2, actual: result.Length);
        }
    }
}
