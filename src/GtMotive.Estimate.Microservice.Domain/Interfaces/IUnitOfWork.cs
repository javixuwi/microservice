using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Repositories;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces
{
    /// <summary>
    /// Unit Of Work. Should only be used by Use Cases.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Gets the repository for managing vehicle-related data and operations.
        /// </summary>
        IVehicleRepository Vehicles { get; }

        /// <summary>
        /// Gets the repository for managing client-related operations.
        /// </summary>
        IClientRepository Clients { get; }

        /// <summary>
        /// Applies all database changes.
        /// </summary>
        /// <returns>Number of affected rows.</returns>
        Task<int> Save();

        /// <summary>
        /// Begins a new asynchronous transaction.
        /// </summary>
        /// <remarks>This method initiates a transaction that can be used to group multiple operations
        /// into a single atomic unit. Ensure that the transaction is committed or rolled back  after use to release
        /// resources.</remarks>
        /// <returns>A task that represents the asynchronous operation of starting the transaction.</returns>
        Task BeginTransactionAsync();
    }
}
