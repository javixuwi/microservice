using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles
{
    /// <summary>
    /// Represents the input data required to create a new vehicle.
    /// </summary>
    /// <remarks>This class encapsulates the details of a vehicle, such as its plate number, brand, model,
    /// manufacturing year, and status information, to be used as input for vehicle creation use cases.</remarks>
    public class CreateInput : IUseCaseInput
    {
        /// <summary>
        /// Gets or sets the license plate number of the vehicle.
        /// </summary>
        public string PlateNumber { get; set; }

        /// <summary>
        /// Gets or sets the brand name of the product.
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets the model name associated with the object.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the item was manufactured.
        /// </summary>
        public DateTime Manufactured { get; set; }
    }
}
