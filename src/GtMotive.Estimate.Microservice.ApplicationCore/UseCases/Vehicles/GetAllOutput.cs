using System.Collections.Generic;
using GtMotive.Estimate.Microservice.Domain.Entities;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles
{
    /// <summary>
    /// Represents the output of a use case that retrieves all vehicles.
    /// </summary>
    /// <remarks>This class encapsulates the collection of vehicles retrieved and the total count of vehicles.
    /// It is typically used as the result of a query or operation that fetches vehicle data.</remarks>
    public class GetAllOutput : IUseCaseOutput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllOutput"/> class.
        /// Represents the output of a query to retrieve all vehicles.
        /// </summary>
        /// <param name="vehicles">The list of vehicles to include in the output. Cannot be null.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="vehicles"/> is null.</exception>
        public GetAllOutput(IList<Vehicle> vehicles)
        {
            Vehicles = new List<Vehicle>(vehicles).AsReadOnly();
            TotalCount = Vehicles.Count;
        }

        /// <summary>
        /// Gets the collection of vehicles currently managed by the system.
        /// </summary>
        public IReadOnlyList<Vehicle> Vehicles { get; }

        /// <summary>
        /// Gets the total number of items in the collection.
        /// </summary>
        public int TotalCount { get; }
    }
}
