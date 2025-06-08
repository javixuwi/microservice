using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles
{
    /// <summary>
    /// Represents a use case for retrieving all vehicles based on specified criteria.
    /// </summary>
    /// <remarks>This use case interacts with the underlying data source to retrieve vehicle information
    /// filtered by rental and availability status. It requires an implementation of <see cref="IUnitOfWork"/> to
    /// perform the data access operations.</remarks>
    public class GetAllUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGetAllOutputPort _outputPort;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllUseCase"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work instance used to manage database transactions and queries. Cannot be <see
        /// langword="null"/>.</param>
        /// <param name="outputPort">The output port instance used to handle the presentation of the retrieved vehicles. Cannot be <see
        /// langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="unitOfWork"/> or <paramref name="outputPort"/> is <see langword="null"/>.</exception>
        public GetAllUseCase(IUnitOfWork unitOfWork, IGetAllOutputPort outputPort)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _outputPort = outputPort ?? throw new ArgumentNullException(nameof(outputPort));
        }

        /// <summary>
        /// Retrieves all vehicles from the data source without applying any filters.
        /// </summary>
        /// <remarks>This method fetches all vehicle records asynchronously and constructs an output
        /// object containing the retrieved vehicles. The output is then passed to the standard output port for further
        /// handling.</remarks>
        /// <returns>A <see cref="GetAllOutput"/> object containing the list of all vehicles.</returns>
        public async Task<GetAllOutput> Execute()
        {
            // Obtener todos los vehículos sin aplicar filtros
            var vehicles = await _unitOfWork.Vehicles.GetAllAsync();

            var output = new GetAllOutput(vehicles);
            _outputPort.StandardHandle(output);

            return output;
        }
    }
}
