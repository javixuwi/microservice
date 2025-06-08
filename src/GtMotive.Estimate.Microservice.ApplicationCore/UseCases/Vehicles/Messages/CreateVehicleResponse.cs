using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.Messages
{
    /// <summary>
    /// Represents the response returned after creating a vehicle.
    /// </summary>
    /// <remarks>This response contains details about the newly created vehicle, including its unique
    /// identifier and license plate number.</remarks>
    public class CreateVehicleResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the license plate number of the vehicle.
        /// </summary>
        public string PlateNumber { get; set; }
    }
}
