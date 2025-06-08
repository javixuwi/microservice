using System;
using MediatR;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.Messages
{
    /// <summary>
    /// Represents a command to create a new vehicle with the specified details.
    /// </summary>
    /// <remarks>This command is used to encapsulate the data required to create a vehicle, including its
    /// license plate number, brand, model, and manufacturing date. It is typically sent to a handler that processes the
    /// creation request and returns a <see cref="CreateVehicleResponse"/>.</remarks>
    public class CreateVehicleCmd : IRequest<CreateVehicleResponse>
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
        /// Gets or sets the model name of the item.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the item was manufactured.
        /// </summary>
        public DateTime Manufactured { get; set; }
    }
}
