using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Repositories;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly IMongoCollection<Client> _clients;

        public ClientRepository(MongoService mongoService)
        {
            if (mongoService == null)
            {
                throw new ArgumentNullException(nameof(mongoService));
            }

            _clients = mongoService.Database.GetCollection<Client>(nameof(Client));
        }

        public async Task<Client> CreateAsync(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            try
            {
                await _clients.InsertOneAsync(client);
                return client;
            }
            catch (MongoException ex)
            {
                throw new InvalidOperationException($"Error creating client: {ex.Message}", ex);
            }
        }

        public async Task<Client> GetByIdNumberAsync(string idNumber)
        {
            var filter = Builders<Client>.Filter.Eq(c => c.IdNumber, idNumber);

            try
            {
                var client = await _clients.Find(filter).FirstOrDefaultAsync();
                return client;
            }
            catch (MongoException ex)
            {
                throw new InvalidOperationException($"Error retrieving client with Identification Number {idNumber}: {ex.Message}", ex);
            }
        }
    }
}
