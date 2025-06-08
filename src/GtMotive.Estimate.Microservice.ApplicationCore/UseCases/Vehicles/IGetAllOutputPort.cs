namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles
{
    /// <summary>
    /// Defines the output port for handling the result of the "Get All Vehicles" operation.
    /// </summary>
    /// <remarks>This interface is used to process the output of the "Get All Vehicles" use case.
    /// Implementations of this interface should define how the result, represented by  <see
    /// cref="GetAllOutput"/>, is handled or presented.</remarks>
    public interface IGetAllOutputPort : IOutputPortStandard<GetAllOutput>
    {
    }
}
