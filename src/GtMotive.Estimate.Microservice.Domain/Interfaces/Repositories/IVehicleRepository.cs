using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Defines a contract for managing vehicle records in the system.
    /// </summary>
    /// <remarks>This interface provides methods for creating, retrieving, and querying vehicle records.
    /// Implementations of this interface are responsible for handling the persistence and retrieval  of <see
    /// cref="Vehicle"/> objects. All methods are asynchronous and designed to support  scalable, non-blocking
    /// operations.</remarks>
    public interface IVehicleRepository
    {
        /// <summary>
        /// Asynchronously creates a new vehicle record in the system.
        /// </summary>
        /// <remarks>The method performs validation on the provided <see cref="Vehicle"/> object before
        /// creating the record.  Ensure that all required properties of the <paramref name="vehicle"/> are populated
        /// before calling this method.</remarks>
        /// <param name="vehicle">The <see cref="Vehicle"/> object containing the details of the vehicle to create. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created <see
        /// cref="Vehicle"/> object, including any system-generated properties such as its unique identifier.</returns>
        Task<Vehicle> CreateAsync(Vehicle vehicle);

        /// <summary>
        /// Asynchronously retrieves all vehicles.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of  <see cref="Vehicle"/>
        /// objects representing all vehicles. If no vehicles are found, the list will be empty.</returns>
        Task<IList<Vehicle>> GetAllAsync();

        /// <summary>
        /// Retrieves a vehicle by its license plate number asynchronously.
        /// </summary>
        /// <param name="plateNumber">The license plate number of the vehicle to retrieve. Cannot be null or empty.</param>
        /// <returns>A <see cref="Vehicle"/> object representing the vehicle with the specified license plate number, or <see
        /// langword="null"/> if no matching vehicle is found.</returns>
        Task<Vehicle> GetByPlateAsync(string plateNumber);

        /// <summary>
        /// Updates the specified vehicle in the system asynchronously.
        /// </summary>
        /// <param name="vehicle">The vehicle to update. Must not be <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated  <see
        /// cref="Vehicle"/> object.</returns>
        Task<Vehicle> UpdateRentalStatusAsync(Vehicle vehicle);

        /// <summary>
        /// Determines whether the client with the specified ID number has rented a vehicle.
        /// </summary>
        /// <param name="clientIdNumber">The unique identifier of the client. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains  <see langword="true"/> if the
        /// client has rented a vehicle; otherwise, <see langword="false"/>.</returns>
        Task<bool> HasClientRentedVehicle(string clientIdNumber);
    }
}
