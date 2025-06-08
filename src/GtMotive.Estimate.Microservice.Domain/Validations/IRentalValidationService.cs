using System.Threading.Tasks;

namespace GtMotive.Estimate.Microservice.Domain.Validations
{
    /// <summary>
    /// Defines methods for validating rental eligibility for clients.
    /// </summary>
    /// <remarks>This service provides functionality to determine whether a client meets the necessary
    /// criteria for renting. Implementations may include checks such as rental history, outstanding balances, or other
    /// eligibility requirements.</remarks>
    public interface IRentalValidationService
    {
        /// <summary>
        /// Validates whether a client is eligible to rent based on their identification number.
        /// </summary>
        /// <remarks>This method performs eligibility checks for a client to ensure they meet the
        /// requirements for renting. The validation process may include checks such as rental history, outstanding
        /// balances, or other criteria.</remarks>
        /// <param name="clientIdNumber">The identification number of the client to validate. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        Task ValidateClientCanRent(string clientIdNumber);
    }
}
