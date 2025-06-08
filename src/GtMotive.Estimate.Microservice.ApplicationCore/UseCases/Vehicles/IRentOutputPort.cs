namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles
{
    /// <summary>
    /// Defines the output port for the vehicle rental operation, providing a mechanism to handle the result of the
    /// operation.
    /// </summary>
    /// <remarks>This interface is used to process the output of a vehicle rental request, typically in a
    /// clean architecture pattern. Implementations of this interface should define how the result of the operation is
    /// presented or handled.</remarks>
    public interface IRentOutputPort : IOutputPortStandard<RentOutput>
    {
    }
}
