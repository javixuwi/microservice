using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Repositories;

namespace GtMotive.Estimate.Microservice.Domain.Validations
{
    /// <summary>
    /// Provides validation services for rental operations, including client eligibility checks.
    /// </summary>
    /// <remarks>This service is designed to validate rental-related criteria, such as client eligibility and
    /// vehicle availability. It relies on an <see cref="IVehicleRepository"/> to access vehicle data for validation
    /// purposes.</remarks>
    public class RentalValidationService : IRentalValidationService
    {
        private readonly IVehicleRepository _vehicleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalValidationService"/> class.
        /// </summary>
        /// <param name="vehicleRepository">The repository used to access vehicle data for validation purposes.  This parameter cannot be <see
        /// langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicleRepository"/> is <see langword="null"/>.</exception>
        public RentalValidationService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository), "Vehicle repository cannot be null.");
        }

        /// <summary>
        /// Validates whether a client is eligible to rent a vehicle.
        /// </summary>
        /// <param name="clientIdNumber">The unique identification number of the client. This value cannot be null, empty, or consist solely of
        /// whitespace.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        /// <exception cref="DomainException">Thrown if <paramref name="clientIdNumber"/> is null, empty, or consists solely of whitespace. Thrown if the
        /// client with the specified ID has already rented a vehicle.</exception>
        public async Task ValidateClientCanRent(string clientIdNumber)
        {
            if (string.IsNullOrWhiteSpace(clientIdNumber))
            {
                throw new DomainException("Client ID number cannot be empty.");
            }

            var hasRentedVehicle = await _vehicleRepository.HasClientRentedVehicle(clientIdNumber);
            if (hasRentedVehicle)
            {
                throw new DomainException($"Client with ID {clientIdNumber} already has a rented vehicle.");
            }
        }
    }
}
