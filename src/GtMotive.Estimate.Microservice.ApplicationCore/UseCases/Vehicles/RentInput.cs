namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles
{
    /// <summary>
    /// Represents the input data required to rent a vehicle.
    /// </summary>
    /// <remarks>This class encapsulates the identifiers for the vehicle and the client involved in the rental
    /// process. It is used as input for use cases that handle vehicle rental operations.</remarks>
    public class RentInput : IUseCaseInput
    {
        /// <summary>
        /// Gets or sets the unique identifier for a vehicle.
        /// </summary>
        public string PlateNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the client.
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the client.
        /// </summary>
        public string ClientEmail { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the client.
        /// </summary>
        public string ClientPhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for a client.
        /// </summary>
        public string ClientIdNumber { get; set; }
    }
}
