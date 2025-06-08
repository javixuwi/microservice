using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Interfaces.Repositories;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly MongoService _mongoService;
        private IClientSessionHandle _session;
        private bool _disposed;

        public UnitOfWork(MongoService mongoService, IVehicleRepository vehicleRepository, IClientRepository clients)
        {
            _mongoService = mongoService;
            Vehicles = vehicleRepository;
            Clients = clients;
        }

        public IVehicleRepository Vehicles { get; }

        public IClientRepository Clients { get; }

        public async Task<int> Save()
        {
            try
            {
                if (_session == null)
                {
                    // If no session is active, we can just return 0 as no changes are made.
                    return 0;
                }

                if (_session.IsInTransaction)
                {
                    await _session.CommitTransactionAsync();
                }

                return 1;
            }
            catch
            {
                if (_session?.IsInTransaction == true)
                {
                    await _session.AbortTransactionAsync();
                }

                throw;
            }
            finally
            {
                if (_session != null)
                {
                    _session.Dispose();
                    _session = null;
                }
            }
        }

        public async Task BeginTransactionAsync()
        {
            if (_session == null)
            {
                _session = await _mongoService.MongoClient.StartSessionAsync();
                try
                {
                    _session.StartTransaction();
                }
                catch (NotSupportedException)
                {
                    _session.Dispose();
                    _session = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _session?.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
