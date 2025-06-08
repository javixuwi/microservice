using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Repositories;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly IMongoCollection<Vehicle> _vehicles;

        public VehicleRepository(MongoService mongoService)
        {
            if (mongoService == null)
            {
                throw new ArgumentNullException(nameof(mongoService));
            }

            _vehicles = mongoService.Database.GetCollection<Vehicle>(nameof(Vehicle));
        }

        public async Task<Vehicle> CreateAsync(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                throw new ArgumentNullException(nameof(vehicle));
            }

            try
            {
                await _vehicles.InsertOneAsync(vehicle);
                return vehicle;
            }
            catch (MongoException ex)
            {
                throw new InvalidOperationException($"Error creating vehicle: {ex.Message}", ex);
            }
        }

        public async Task<IList<Vehicle>> GetAllAsync()
        {
            try
            {
                var filter = Builders<Vehicle>.Filter.Eq(v => v.IsActive, true);

                var vehicles = await _vehicles
                    .Find(filter)
                    .ToListAsync();

                return vehicles;
            }
            catch (MongoException ex)
            {
                throw new InvalidOperationException($"Error retrieving vehicles: {ex.Message}", ex);
            }
        }

        public async Task<Vehicle> GetByPlateAsync(string plateNumber)
        {
            var filter = Builders<Vehicle>.Filter.Eq(v => v.PlateNumber, plateNumber);

            try
            {
                var vehicle = await _vehicles.Find(filter).FirstOrDefaultAsync();

                return vehicle;
            }
            catch (MongoException ex)
            {
                throw new InvalidOperationException($"Error retrieving vehicle by plate number: {ex.Message}", ex);
            }
        }

        public async Task<bool> HasClientRentedVehicle(string clientIdNumber)
        {
            try
            {
                var filter = Builders<Vehicle>.Filter.And(
                    Builders<Vehicle>.Filter.Eq(v => v.IsActive, true),
                    Builders<Vehicle>.Filter.Eq(v => v.IsRented, true),
                    Builders<Vehicle>.Filter.Eq("Client.IdNumber", clientIdNumber));

                return await _vehicles.CountDocumentsAsync(filter) > 0;
            }
            catch (MongoException ex)
            {
                throw new InvalidOperationException($"Error checking client's rented vehicles: {ex.Message}", ex);
            }
        }

        public async Task<Vehicle> UpdateRentalStatusAsync(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                throw new ArgumentNullException(nameof(vehicle));
            }

            try
            {
                var filter = Builders<Vehicle>.Filter.Eq(v => v.Id, vehicle.Id);
                var updateResult = await _vehicles.ReplaceOneAsync(filter, vehicle);

                return updateResult.MatchedCount == 0
                    ? throw new InvalidOperationException($"Vehicle with ID {vehicle.Id} does not exist")
                    : vehicle;
            }
            catch (MongoException ex)
            {
                throw new InvalidOperationException($"Error updating vehicle: {ex.Message}", ex);
            }
        }
    }
}
