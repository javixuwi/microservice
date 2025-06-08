using System;
using System.Threading.Tasks;

namespace GtMotive.Estimate.Microservice.Domain.Validations
{
    /// <summary>
    /// Defines methods for validating vehicle-related data.
    /// </summary>
    /// <remarks>This service provides validation logic for vehicle data, such as ensuring vehicles meet
    /// specific criteria based on their manufacturing date or other attributes.</remarks>
    public interface IVehicleValidationsService
    {
        /// <summary>
        /// Checks if the vehicle is valid for the given date. Not valid if the vehicle has been manufactured more than 5 years from now.
        /// </summary>
        /// <param name="manufacturationDate">Date of manufacturated.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        Task ValidateVehicleAge(DateTime manufacturationDate);
    }
}
