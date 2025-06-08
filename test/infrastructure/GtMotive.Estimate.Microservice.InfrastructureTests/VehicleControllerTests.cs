using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles;
using GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure;
using Newtonsoft.Json;
using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests
{
    public class VehicleControllerTests : InfrastructureTestBase
    {
        private readonly HttpClient _client;

        public VehicleControllerTests(GenericInfrastructureTestServerFixture fixture)
            : base(fixture)
        {
            _client = Fixture.Server.CreateClient();
        }

        [Fact]
        public async Task CreateWithValidDataReturnsCreated()
        {
            // Arrange
            var input = new CreateInput
            {
                PlateNumber = "1234ABC",
                Brand = "Toyota",
                Model = "Corolla",
                Manufactured = new DateTime(2023, 1, 1)
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, "/api/vehicle");
            using var content = new StringContent(
                JsonConvert.SerializeObject(input),
                Encoding.UTF8,
                "application/json");
            request.Content = content;

            // Act
            using var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData("", "Brand", "Model")]
        [InlineData("1234ABC", "", "Model")]
        [InlineData("1234ABC", "Brand", "")]
        public async Task CreateWithInvalidDataReturnsBadRequest(
            string plateNumber,
            string brand,
            string model)
        {
            // Arrange
            var input = new CreateInput
            {
                PlateNumber = plateNumber,
                Brand = brand,
                Model = model,
                Manufactured = DateTime.Now
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, "/api/vehicle");
            using var content = new StringContent(
                JsonConvert.SerializeObject(input),
                Encoding.UTF8,
                "application/json");
            request.Content = content;

            // Act
            using var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(responseContent);
        }
    }
}
