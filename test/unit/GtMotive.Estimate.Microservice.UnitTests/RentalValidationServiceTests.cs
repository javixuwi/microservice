using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Repositories;
using GtMotive.Estimate.Microservice.Domain.Validations;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests
{
    public class RentalValidationServiceTests
    {
        private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
        private readonly RentalValidationService _sut;

        public RentalValidationServiceTests()
        {
            _vehicleRepositoryMock = new Mock<IVehicleRepository>();
            _sut = new RentalValidationService(_vehicleRepositoryMock.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ValidateClientCanRentWithInvalidClientIdThrowsDomainException(string clientId)
        {
            var exception = await Assert.ThrowsAsync<DomainException>(
                () => _sut.ValidateClientCanRent(clientId));

            Assert.Equal("Client ID number cannot be empty.", exception.Message);
            _vehicleRepositoryMock.Verify(x => x.HasClientRentedVehicle(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ValidateClientCanRentWhenClientHasRentedVehicleThrowsDomainException()
        {
            var clientId = "12345678A";
            _vehicleRepositoryMock
                .Setup(x => x.HasClientRentedVehicle(clientId))
                .ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<DomainException>(
                () => _sut.ValidateClientCanRent(clientId));

            Assert.Equal($"Client with ID {clientId} already has a rented vehicle.", exception.Message);
            _vehicleRepositoryMock.Verify(x => x.HasClientRentedVehicle(clientId), Times.Once);
        }

        [Fact]
        public async Task ValidateClientCanRentWhenClientHasNoRentedVehicleSucceeds()
        {
            var clientId = "12345678A";
            _vehicleRepositoryMock
                .Setup(x => x.HasClientRentedVehicle(clientId))
                .ReturnsAsync(false);

            await _sut.ValidateClientCanRent(clientId);

            _vehicleRepositoryMock.Verify(x => x.HasClientRentedVehicle(clientId), Times.Once);
        }
    }
}
