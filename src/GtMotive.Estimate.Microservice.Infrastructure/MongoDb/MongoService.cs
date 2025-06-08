using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using Polly;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb
{
    public class MongoService
    {
        private const int RetryCount = 5;
        private const int InitialRetryDelaySeconds = 1;

        private static readonly Action<ILogger, int, double, Exception> LogRetryAttempt =
            LoggerMessage.Define<int, double>(
                LogLevel.Warning,
                new EventId(1, nameof(LogRetryAttempt)),
                "Retry attempt {RetryAttempt} to connect to MongoDB after {Delay}s");

        private static readonly Action<ILogger, int, int, Exception> LogConnectionError =
            LoggerMessage.Define<int, int>(
                LogLevel.Error,
                new EventId(2, nameof(LogConnectionError)),
                "Error connecting to MongoDB (Attempt {RetryCount} of {MaxRetries})");

        private static readonly Action<ILogger, string, Exception> LogConnectionAttempt =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId(3, nameof(LogConnectionAttempt)),
                "Attempting to connect to MongoDB at {ConnectionString}");

        private static readonly Action<ILogger, string, Exception> LogSuccessfulConnection =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId(4, nameof(LogSuccessfulConnection)),
                "Successfully connected to MongoDB database {DatabaseName}");

        private readonly ILogger<MongoService> _logger;

        public MongoService(IOptions<MongoDbSettings> options, ILogger<MongoService> logger)
        {
            _logger = logger;

            var policy = Policy
                .Handle<MongoConnectionException>()
                .Or<TimeoutException>()
                .WaitAndRetryAsync(
                    RetryCount,
                    retryAttempt =>
                    {
                        var delay = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt) * InitialRetryDelaySeconds);
                        LogRetryAttempt(_logger, retryAttempt, delay.TotalSeconds, null);
                        return delay;
                    },
                    (exception, timeSpan, retryCount, context) =>
                    {
                        LogConnectionError(_logger, retryCount, RetryCount, exception);
                    });

            policy.ExecuteAsync(async () =>
            {
                LogConnectionAttempt(_logger, options.Value.ConnectionString, null);

                MongoClient = new MongoClient(options.Value.ConnectionString);
                Database = MongoClient.GetDatabase(options.Value.MongoDbDatabaseName);

                await Database.RunCommandAsync((Command<BsonDocument>)$"{{ping:1}}");

                LogSuccessfulConnection(_logger, options.Value.MongoDbDatabaseName, null);

                await EnsureDatabaseSetupAsync();
                RegisterBsonClasses();
            }).GetAwaiter().GetResult();
        }

        public MongoClient MongoClient { get; private set; }

        public IMongoDatabase Database { get; private set; }

        private static void RegisterBsonClasses()
        {
            var pack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreIfNullConvention(true),
                new EnumRepresentationConvention(BsonType.String)
            };
            ConventionRegistry.Register("CustomConventions", pack, t => true);

            if (!BsonClassMap.IsClassMapRegistered(typeof(Client)))
            {
                BsonClassMap.RegisterClassMap<Client>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                    cm.MapIdMember(c => c.Id)
                        .SetIdGenerator(GuidGenerator.Instance);
                    cm.MapMember(c => c.Name).SetIsRequired(true);
                    cm.MapMember(c => c.Email).SetIsRequired(true);
                    cm.MapMember(c => c.PhoneNumber).SetIsRequired(true);
                    cm.MapMember(c => c.IdNumber).SetIsRequired(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Vehicle)))
            {
                BsonClassMap.RegisterClassMap<Vehicle>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                    cm.MapIdMember(c => c.Id)
                        .SetIdGenerator(GuidGenerator.Instance);
                    cm.MapMember(c => c.PlateNumber).SetIsRequired(true);
                    cm.MapMember(c => c.Brand).SetIsRequired(true);
                    cm.MapMember(c => c.Model).SetIsRequired(true);
                    cm.MapMember(c => c.Manufactured).SetIsRequired(true);
                    cm.MapMember(c => c.Registration)
                        .SetDefaultValue(DateTime.UtcNow);
                    cm.MapMember(c => c.IsActive)
                        .SetDefaultValue(true);
                    cm.MapMember(c => c.IsRented)
                        .SetDefaultValue(false);

                    cm.MapMember(c => c.Client);
                });
            }
        }

        private async Task EnsureDatabaseSetupAsync()
        {
            var collections = await Database.ListCollectionNamesAsync();
            var collectionsList = await collections.ToListAsync();

            if (!collectionsList.Contains("Client"))
            {
                await Database.CreateCollectionAsync("Client");
                var clientsCollection = Database.GetCollection<Client>("Client");
                var idNumberIndexKeysDefinition = Builders<Client>.IndexKeys
                    .Ascending(c => c.IdNumber);
                var idNumberIndexOptions = new CreateIndexOptions { Unique = true };
                var idNumberIndexModel = new CreateIndexModel<Client>(
                    idNumberIndexKeysDefinition,
                    idNumberIndexOptions);
                await clientsCollection.Indexes.CreateOneAsync(idNumberIndexModel);
            }

            if (!collectionsList.Contains("Vehicle"))
            {
                await Database.CreateCollectionAsync("Vehicle");
                var vehiclesCollection = Database.GetCollection<Vehicle>("Vehicle");

                var plateIndexKeysDefinition = Builders<Vehicle>.IndexKeys
                    .Ascending(v => v.PlateNumber);
                var plateIndexOptions = new CreateIndexOptions { Unique = true };
                var plateIndexModel = new CreateIndexModel<Vehicle>(
                    plateIndexKeysDefinition,
                    plateIndexOptions);
                await vehiclesCollection.Indexes.CreateOneAsync(plateIndexModel);
            }
        }
    }
}
