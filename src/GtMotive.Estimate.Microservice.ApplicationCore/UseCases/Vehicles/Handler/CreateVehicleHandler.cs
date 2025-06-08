using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.Messages;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Repositories;
using GtMotive.Estimate.Microservice.Domain.Validations;
using MediatR;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.Handler
{
    /// <summary>
    /// Handles the creation of a new vehicle by processing a <see cref="CreateVehicleCmd"/> command and returning a
    /// <see cref="CreateVehicleResponse"/> containing the details of the created vehicle.
    /// </summary>
    /// <remarks>This handler validates the input command, checks for duplicate vehicles based on the plate
    /// number, and persists the new vehicle to the repository. If the vehicle already exists or the input is invalid,
    /// appropriate exceptions are thrown.</remarks>
    public class CreateVehicleHandler : IRequestHandler<CreateVehicleCmd, CreateVehicleResponse>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IVehicleValidationsService _validationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateVehicleHandler"/> class.
        /// </summary>
        /// <param name="vehicleRepository">The repository used to manage vehicle data.</param>
        /// <param name="validationService">The service used to perform vehicle-related validations.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicleRepository"/> or <paramref name="validationService"/> is <see
        /// langword="null"/>.</exception>
        public CreateVehicleHandler(
            IVehicleRepository vehicleRepository,
            IVehicleValidationsService validationService)
        {
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
            _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
        }

        /// <summary>
        /// Handles the creation of a new vehicle based on the provided command.
        /// </summary>
        /// <remarks>This method validates the vehicle's data, checks for duplicate plate numbers, and
        /// persists the new vehicle to the repository. Ensure that the <paramref name="request"/> contains valid data
        /// before calling this method.</remarks>
        /// <param name="request">The command containing the details of the vehicle to be created. Cannot be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="CreateVehicleResponse"/> containing the details of the newly created vehicle.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if a vehicle with the same plate number as specified in <paramref name="request"/> already exists.</exception>
        public async Task<CreateVehicleResponse> Handle(CreateVehicleCmd request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // Validar datos
            await _validationService.ValidateVehicleAge(request.Manufactured);

            // Verificar si ya existe
            var existing = await _vehicleRepository.GetByPlateAsync(request.PlateNumber);
            if (existing != null)
            {
                throw new InvalidOperationException($"Vehicle with plate {request.PlateNumber} already exists");
            }

            // Crear el vehículo
            var vehicle = new Vehicle(
                request.PlateNumber,
                request.Brand,
                request.Model,
                request.Manufactured);

            // Persistir
            var created = await _vehicleRepository.CreateAsync(vehicle);

            // Retornar respuesta
            return new CreateVehicleResponse
            {
                Id = created.Id,
                PlateNumber = created.PlateNumber
            };
        }
    }
}
