using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.Messages;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests
{
    public class CreateVehicleTests : FunctionalTestBase
    {
        public CreateVehicleTests(CompositionRootTestFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public async Task CreateVehicleWithDuplicatePlateThrowsException()
        {
            var command = new CreateVehicleCmd
            {
                PlateNumber = "1234ABC",
                Brand = "Toyota",
                Model = "Corolla",
                Manufactured = new DateTime(2023, 1, 1)
            };

            await Fixture.UsingHandlerForRequestResponse<CreateVehicleCmd, CreateVehicleResponse>(async handler =>
            {
                var result = await handler.Handle(command, default);
                Assert.NotNull(result);
            });

            await Fixture.UsingHandlerForRequestResponse<CreateVehicleCmd, CreateVehicleResponse>(async handler =>
            {
                await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                {
                    await handler.Handle(command, default);
                });
            });
        }

        [Theory]
        [InlineData("", "Brand", "Model")]
        [InlineData("1234ABC", "", "Model")]
        [InlineData("1234ABC", "Brand", "")]
        public async Task CreateVehicleWithInvalidDataThrowsException(
            string plateNumber,
            string brand,
            string model)
        {
            var command = new CreateVehicleCmd
            {
                PlateNumber = plateNumber,
                Brand = brand,
                Model = model,
                Manufactured = DateTime.Now
            };

            await Fixture.UsingHandlerForRequestResponse<CreateVehicleCmd, CreateVehicleResponse>(async handler =>
            {
                await Assert.ThrowsAsync<DomainException>(async () =>
                {
                    await handler.Handle(command, default);
                });
            });
        }
    }
}
