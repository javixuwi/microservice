using System;
using System.Threading.Tasks;

namespace GtMotive.Estimate.Microservice.Domain.Validations
{
    /// <summary>
    /// Provides validation services for vehicle-related data, ensuring compliance with specific business rules.
    /// </summary>
    /// <remarks>This service includes methods to validate various aspects of vehicle data, such as
    /// manufacturing dates. It is designed to enforce constraints that ensure vehicles meet predefined
    /// criteria.</remarks>
    public class VehicleValidationService : IVehicleValidationsService
    {
        private const int MaxVehicleAgeInYears = 5;

        /// <summary>
        /// Validates the age of a vehicle based on its manufacturing date.
        /// </summary>
        /// <remarks>This method checks the validity of the manufacturing date and ensures the vehicle's
        /// age does not exceed the maximum allowed age. If the manufacturing date is invalid or the vehicle's age is
        /// too high, a <see cref="DomainException"/> is thrown.</remarks>
        /// <param name="manufacturationDate">The manufacturing date of the vehicle. Must be a valid date that is not in the future.</param>
        /// <returns>A completed task if the vehicle's age is within the allowed range.</returns>
        /// <exception cref="DomainException">Thrown if <paramref name="manufacturationDate"/> is not provided, is in the future, or if the vehicle's age
        /// exceeds the maximum allowed age.</exception>
        public Task ValidateVehicleAge(DateTime manufacturationDate)
        {
            if (manufacturationDate == default)
            {
                throw new DomainException("Manufacturing date must be provided.");
            }

            if (manufacturationDate > DateTime.UtcNow)
            {
                throw new DomainException("Manufacturing date cannot be in the future.");
            }

            var age = CalculateVehicleAge(manufacturationDate);

            return age >= MaxVehicleAgeInYears
                ? throw new DomainException(
                    $"Vehicle age ({age} years) exceeds the maximum allowed age of {MaxVehicleAgeInYears} years.")
                : Task.CompletedTask;
        }

        /// <summary>
        /// Calculates the age of a vehicle based on its manufacturing date.
        /// </summary>
        /// <param name="manufacturationDate">The date when the vehicle was manufactured.</param>
        /// <returns>The age of the vehicle in years.</returns>
        private static int CalculateVehicleAge(DateTime manufacturationDate)
        {
            var today = DateTime.UtcNow;
            var age = today.Year - manufacturationDate.Year;

            if (manufacturationDate.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}
