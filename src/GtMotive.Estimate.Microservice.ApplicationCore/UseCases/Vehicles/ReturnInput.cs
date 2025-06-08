namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles
{
    /// <summary>
    /// Represents the input data required for a use case that processes vehicle return operations.
    /// </summary>
    /// <remarks>This class is used to encapsulate the necessary information for handling vehicle returns. It
    /// includes the vehicle's plate number, which uniquely identifies the vehicle being returned.</remarks>
    public class ReturnInput : IUseCaseInput
    {
        /// <summary>
        /// Gets or sets the vehicle plate number.
        /// </summary>
        public string PlateNumber { get; set; }
    }
}
