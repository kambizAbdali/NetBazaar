using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NetBazaar.Application.Common.Configuration;
using NetBazaar.Persistence.Interfaces.DatabaseContext;

namespace NetBazaar.Persistence.Data
{
    public class MongoDbContext<T> : IMongoDbContext<T>
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<T> _collection;

        public MongoDbContext(IOptions<MongoDBSettings> options)
        {
            if (options?.Value == null)
                throw new ArgumentNullException(nameof(options), "MongoDB settings cannot be null");

            var settings = options.Value;

            if (string.IsNullOrWhiteSpace(settings.ConnectionString))
                throw new ArgumentException("MongoDB connection string cannot be null or empty");

            if (string.IsNullOrWhiteSpace(settings.DatabaseName))
                throw new ArgumentException("MongoDB database name cannot be null or empty");

            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
            _collection = _database.GetCollection<T>(GetCollectionName(typeof(T)));
        }

        public IMongoCollection<T> GetCollection()
        {
            return _collection;
        }

        private static string GetCollectionName(Type documentType)
        {
            return documentType.Name;
        }
    }
}