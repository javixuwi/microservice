using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Defines a repository for managing client entities.
    /// </summary>
    /// <remarks>This interface provides methods for performing CRUD operations on client entities.
    /// Implementations of this interface are responsible for handling data persistence and retrieval.</remarks>
    public interface IClientRepository
    {
        /// <summary>
        /// Asynchronously creates a new client.
        /// </summary>
        /// <param name="client">The client to create.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created client.</returns>
        Task<Client> CreateAsync(Client client);

        /// <summary>
        /// Retrieves a client by its unique identifier asynchronously.
        /// </summary>
        /// <param name="idNumber">The unique identifier of the client to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the  <see cref="Client"/> object
        /// corresponding to the specified identifier, or <see langword="null"/>  if no client with the given identifier
        /// exists.</returns>
        Task<Client> GetByIdNumberAsync(string idNumber);
    }
}
