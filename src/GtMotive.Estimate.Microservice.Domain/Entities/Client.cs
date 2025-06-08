using System;

namespace GtMotive.Estimate.Microservice.Domain.Entities
{
    /// <summary>
    /// Represents a client entity with identifying information such as name, email, phone number, and ID number.
    /// </summary>
    /// <remarks>The <see cref="Client"/> class is designed to encapsulate the details of a client, including
    /// their unique identifier. Instances of this class are immutable after creation, ensuring the integrity of the
    /// client's data.</remarks>
    public class Client
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class with the specified name, email, phone number,
        /// and ID number.
        /// </summary>
        /// <remarks>This constructor automatically generates a unique identifier for the client. Ensure
        /// that the provided parameters meet the validation requirements to avoid exceptions.</remarks>
        /// <param name="name">The name of the client. Cannot be null or empty.</param>
        /// <param name="email">The email address of the client. Must be a valid email format.</param>
        /// <param name="phoneNumber">The phone number of the client. Cannot be null or empty.</param>
        /// <param name="idNumber">The identification number of the client. Cannot be null or empty.</param>
        public Client(string name, string email, string phoneNumber, string idNumber)
        {
            Validate(name, email, phoneNumber, idNumber);

            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            IdNumber = idNumber;
        }

        private Client()
        {
        }

        /// <summary>
        /// Gets the unique identifier for the entity.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the name associated with the current instance.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the email address associated with the user.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Gets the phone number associated with the current instance.
        /// </summary>
        public string PhoneNumber { get; private set; }

        /// <summary>
        /// Gets the unique identifier number associated with the entity.
        /// </summary>
        public string IdNumber { get; private set; }

        private static void Validate(string name, string email, string phoneNumber, string idNumber)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainException("Name cannot be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new DomainException("Email cannot be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new DomainException("Phone number cannot be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(idNumber))
            {
                throw new DomainException("ID number cannot be null or empty.");
            }
        }
    }
}
