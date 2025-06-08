using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Validations;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles
{
    /// <summary>
    /// Handles the creation of a new vehicle in the system.
    /// </summary>
    /// <remarks>This use case ensures that a vehicle with the specified plate number does not already exist
    /// before creating a new vehicle. It performs the operation within a transactional context to ensure data
    /// consistency. If a vehicle with the same plate number already exists, an exception is thrown.</remarks>
    public class CreateUseCase : IUseCase<CreateInput>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVehicleValidationsService _vehicleValidationsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUseCase"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work instance used to manage database transactions and operations. Cannot be <see
        /// langword="null"/>.</param>
        /// <param name="vehicleValidationsService">The service responsible for validating vehicle-related data. Cannot be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="unitOfWork"/> or <paramref name="vehicleValidationsService"/> is <see
        /// langword="null"/>.</exception>
        public CreateUseCase(IUnitOfWork unitOfWork, IVehicleValidationsService vehicleValidationsService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _vehicleValidationsService = vehicleValidationsService ?? throw new ArgumentNullException(nameof(vehicleValidationsService));
        }

        /// <summary>
        /// Creates a new vehicle and persists it to the database.
        /// </summary>
        /// <remarks>This method begins a transaction, validates that a vehicle with the specified plate
        /// number does not already exist,  and then creates and saves the new vehicle. If a vehicle with the same plate
        /// number already exists, an exception is thrown.</remarks>
        /// <param name="input">The input data required to create the vehicle, including plate number, brand, model, and manufacturing date.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a vehicle with the specified plate number already exists in the database.</exception>
        public async Task<int> Execute(CreateInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            await _unitOfWork.BeginTransactionAsync();

            await _vehicleValidationsService.ValidateVehicleAge(input.Manufactured);

            var vehicle = new Vehicle(input.PlateNumber, input.Brand, input.Model, input.Manufactured);

            var existing = await _unitOfWork.Vehicles.GetByPlateAsync(input.PlateNumber);
            if (existing != null)
            {
                throw new InvalidOperationException($"Vehicle with plate {input.PlateNumber} already exists");
            }

            await _unitOfWork.Vehicles.CreateAsync(vehicle);

            var result = await _unitOfWork.Save();
            return result;
        }
    }
}
