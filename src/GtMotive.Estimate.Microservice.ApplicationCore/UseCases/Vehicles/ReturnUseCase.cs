using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles
{
    /// <summary>
    /// Represents a use case for processing the return of a vehicle.
    /// </summary>
    /// <remarks>This use case handles the return of a vehicle by updating its state and persisting the
    /// changes. It requires a valid <see cref="ReturnInput"/> object containing the vehicle's plate number.</remarks>
    public class ReturnUseCase : IUseCase<ReturnInput>
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnUseCase"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work instance used to manage database transactions and operations.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="unitOfWork"/> is <see langword="null"/>.</exception>
        public ReturnUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <summary>
        /// Processes the return of a vehicle based on the provided input.
        /// </summary>
        /// <remarks>This method retrieves a vehicle by its plate number, processes its return, and saves
        /// the changes. Ensure that the input contains a valid plate number for a registered vehicle.</remarks>
        /// <param name="input">The input containing the vehicle's plate number and other return details. Cannot be <see langword="null"/>.</param>
        /// <returns>The result of the operation as an integer. Typically indicates the status or outcome of the return process.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="input"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if no vehicle is found with the specified plate number.</exception>
        public async Task<int> Execute(ReturnInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input), "Input cannot be null");
            }

            var vehicle = await _unitOfWork.Vehicles.GetByPlateAsync(input.PlateNumber) ?? throw new InvalidOperationException($"Vehicle with ID {input.PlateNumber} not found.");
            if (!vehicle.IsRented)
            {
                throw new InvalidOperationException($"Vehicle with ID {input.PlateNumber} is not currently rented.");
            }

            if (vehicle.Client == null)
            {
                throw new InvalidOperationException($"Vehicle with ID {input.PlateNumber} has no associated client.");
            }

            vehicle.Return();
            await _unitOfWork.Vehicles.UpdateRentalStatusAsync(vehicle);

            return await _unitOfWork.Save();
        }
    }
}
