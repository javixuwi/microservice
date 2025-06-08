using System;

namespace GtMotive.Estimate.Microservice.Domain.Entities
{
    /// <summary>
    /// Represents a vehicle with associated details such as plate number, brand, model,  manufacturing date, and
    /// registration date.
    /// </summary>
    /// <remarks>This class provides a unique identifier for each vehicle instance and ensures that  all
    /// required details are validated during initialization. Instances of this class  are immutable after
    /// creation.</remarks>
    public class Vehicle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vehicle"/> class.
        /// </summary>
        /// <param name="plateNumber">Plate number associated with the vehicle.</param>
        /// <param name="brand">Brand associated with the vehicle.</param>
        /// <param name="model">Model Associated with the brand of the vehicle.</param>
        /// <param name="manufactured">Date of manufactoration of the vehicle.</param>
        /// <param name="registered">Date of registration by the company.</param>
        public Vehicle(string plateNumber, string brand, string model, DateTime manufactured)
        {
            Validate(plateNumber, brand, model, manufactured);

            Id = Guid.NewGuid();
            PlateNumber = plateNumber;
            Brand = brand;
            Model = model;
            Manufactured = manufactured;

            Registration = DateTime.UtcNow;
        }

        private Vehicle()
        {
        }

        /// <summary>
        /// Gets the unique identifier for the entity.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the license plate number associated with the vehicle.
        /// </summary>
        public string PlateNumber { get; private set; }

        /// <summary>
        /// Gets the brand name of the product.
        /// </summary>
        public string Brand { get; private set; }

        /// <summary>
        /// Gets the model name associated with the current instance.
        /// </summary>
        public string Model { get; private set; }

        /// <summary>
        /// Gets the date and time when the item was manufactured.
        /// </summary>
        public DateTime Manufactured { get; private set; }

        /// <summary>
        /// Gets the date and time when the entity was registered at the company.
        /// </summary>
        public DateTime Registration { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the current instance is active. Should not be confused with the rental status.
        /// </summary>
        public bool IsActive { get; private set; } = true;

        /// <summary>
        /// Gets a value indicating whether the item is currently rented.
        /// </summary>
        public bool IsRented { get; private set; }

        /// <summary>
        /// Gets the client instance used to interact with the underlying service or system.
        /// </summary>
        public Client Client { get; private set; }

        /// <summary>
        /// Updates the rental status of a client.
        /// </summary>
        /// <param name="client">The client whose rental status is being updated. Cannot be <see langword="null"/>.</param>
        /// <param name="rented">A value indicating whether the client has rented the item.  <see langword="true"/> if the client has rented;
        /// otherwise, <see langword="false"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="client"/> is <see langword="null"/>.</exception>
        public void UpdateRentStatus(Client client, bool rented)
        {
            Client = !rented ? null : client ?? throw new ArgumentNullException(nameof(client), "Client cannot be null.");
            IsRented = rented;
        }

        /// <summary>
        /// Updates the active status of the current instance.
        /// </summary>
        /// <param name="isActive">A value indicating whether the instance should be marked as active.  <see langword="true"/> to set the
        /// instance as active; otherwise, <see langword="false"/>.</param>
        public void Return()
        {
            Client = null;
            IsActive = false;
        }

        /// <summary>
        /// Validation method for the Vehicle entity.
        /// </summary>
        /// <param name="plateNumber">Plate number associated with the vehicle.</param>
        /// <param name="brand">Brand associated with the vehicle.</param>
        /// <param name="model">Model Associated with the brand of the vehicle.</param>
        /// <param name="manufactured">Date of manufactoration of the vehicle.</param>
        /// <exception cref="ArgumentException">If any parameters does not validated.</exception>
        private static void Validate(string plateNumber, string brand, string model, DateTime manufactured)
        {
            if (string.IsNullOrWhiteSpace(plateNumber))
            {
                throw new DomainException("Plate number cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(brand))
            {
                throw new DomainException("Brand cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(model))
            {
                throw new DomainException("Model cannot be empty.");
            }

            if (manufactured == DateTime.MinValue || manufactured == DateTime.MaxValue)
            {
                throw new DomainException("Manufactured date is invalid.");
            }
        }
    }
}
