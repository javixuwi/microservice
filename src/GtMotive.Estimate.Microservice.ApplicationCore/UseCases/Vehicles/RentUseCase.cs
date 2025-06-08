using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles
{
    /// <summary>
    /// Represents a use case for renting a vehicle.
    /// </summary>
    /// <remarks>This use case encapsulates the logic required to process a vehicle rental operation. It
    /// depends on an <see cref="IUnitOfWork"/> instance to manage transactional operations.</remarks>
    public class RentUseCase : IUseCase<RentInput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRentOutputPort _outputPort;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentUseCase"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work instance used to manage transactions and data persistence. This parameter cannot be <see
        /// langword="null"/>.</param>
        /// <param name="outputPort">The output port instance used to handle the presentation of results. This parameter cannot be <see
        /// langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="unitOfWork"/> or <paramref name="outputPort"/> is <see langword="null"/>.</exception>
        public RentUseCase(IUnitOfWork unitOfWork, IRentOutputPort outputPort)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _outputPort = outputPort ?? throw new ArgumentNullException(nameof(outputPort));
        }

        /// <summary>
        /// Executes the process of renting a vehicle to a client.
        /// </summary>
        /// <remarks>This method performs the following steps: <list type="bullet"> <item>Begins a
        /// database transaction.</item> <item>Validates the vehicle's availability and rental status.</item>
        /// <item>Ensures the client exists in the system, creating a new client if necessary.</item> <item>Updates the
        /// vehicle's rental status and associates it with the client.</item> <item>Commits the changes to the
        /// database.</item> </list> If the operation completes successfully, the output is handled via the configured
        /// output port.</remarks>
        /// <param name="input">The input data required to rent a vehicle, including the vehicle's plate number and client details. Cannot
        /// be <see langword="null"/>.</param>
        /// <returns>The number of entities affected in the database as a result of the operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="input"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the vehicle with the specified plate number does not exist, is inactive, or is already rented.</exception>
        public async Task<int> Execute(RentInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            await _unitOfWork.BeginTransactionAsync();

            var hasRentedVehicle = await _unitOfWork.Vehicles.HasClientRentedVehicle(input.ClientIdNumber);
            if (hasRentedVehicle)
            {
                throw new InvalidOperationException($"Client with ID {input.ClientIdNumber} already has a rented vehicle");
            }

            var vehicle = await _unitOfWork.Vehicles.GetByPlateAsync(input.PlateNumber) ?? throw new InvalidOperationException($"Vehicle with plate {input.PlateNumber} does not exist");

            if (!vehicle.IsActive)
            {
                throw new InvalidOperationException($"Vehicle with plate {input.PlateNumber} is not available for rent");
            }

            if (vehicle.IsRented)
            {
                throw new InvalidOperationException($"Vehicle with plate {input.PlateNumber} is already rented");
            }

            var client = await _unitOfWork.Clients.GetByIdNumberAsync(input.ClientIdNumber);
            if (client == null)
            {
                client = new Client(input.ClientName, input.ClientEmail, input.ClientPhoneNumber, input.ClientIdNumber);
                await _unitOfWork.Clients.CreateAsync(client);
            }

            vehicle.UpdateRentStatus(client, true);

            await _unitOfWork.Vehicles.UpdateRentalStatusAsync(vehicle);

            var result = await _unitOfWork.Save();

            var output = new RentOutput(vehicle);
            _outputPort.StandardHandle(output);

            return result;
        }
    }
}
