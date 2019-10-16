using AutenticationAPI;
using AutenticationAPI.Models;
using AutenticationAPI.Services;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitTestIntegrationAPI.Mocks;

namespace UnitTestIntegrationAPI
{
    [TestClass]
    public class CardControllerTest
    {

        private WebApplicationFactory<Startup> _factory;

        [TestInitialize]
        public void Initialize()
        {
            _factory = new WebApplicationFactory<Startup>();
        }

        // Overrided the Startup file. Services, filters .....
        public WebApplicationFactory<Startup> CreateWebHostingBuilderSetup()
        {
            return _factory.WithWebHostBuilder(builder =>
           {
               builder.ConfigureTestServices(services =>
               {
                   services.AddMvc(options =>
                   {
                       options.Filters.Add(new AllowAnonymousFilter());
                   });
                  services.AddScoped<ICardService, CardServiceMock>();
                  
               });
           });
        }

        [TestMethod]
        public async Task GetCardNotCarExist_Return404()
        {
            // Arrange
            var client = this.CreateWebHostingBuilderSetup().CreateClient();
            var url = "/api/card/0";

            //Act
            var response = await client.GetAsync(url);

            //Assert
            Assert.AreEqual(expected: 404, actual: (int)response.StatusCode);
        }

        [TestMethod]
        public async Task GetCardExist_ReturnCard()
        {
            // Arrange
            var client = this.CreateWebHostingBuilderSetup().CreateClient();
            var url = "/api/card/2";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Assert.IsTrue(false, "No Found" + response.StatusCode);
            }

            //Act
            var result = JsonConvert.DeserializeObject<Card>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected: 2, actual: result.CardId);
            Assert.AreEqual(expected: "Alex", actual: result.CardOwnerName);
        }

    }
}
