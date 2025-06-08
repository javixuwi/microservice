using System;
using GtMotive.Estimate.Microservice.Domain.Entities;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles
{
    /// <summary>
    /// Represents the output of a vehicle rental operation, containing the rented vehicle.
    /// </summary>
    /// <remarks>This class is used to encapsulate the result of a vehicle rental use case, providing access
    /// to the rented vehicle. It ensures that the vehicle is always non-null, as it is a required part of the rental
    /// operation.</remarks>
    public class RentOutput : IUseCaseOutput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RentOutput"/> class with the specified vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle to be rented. Cannot be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicle"/> is <see langword="null"/>.</exception>
        public RentOutput(Vehicle vehicle)
        {
            Vehicle = vehicle ?? throw new ArgumentNullException(nameof(vehicle), "Vehicle cannot be null");
        }

        /// <summary>
        /// Gets the vehicle associated with the current context.
        /// </summary>
        public Vehicle Vehicle { get; }
    }
}
