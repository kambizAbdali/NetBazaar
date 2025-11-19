using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Persistence.Interfaces.DatabaseContext
{
    public interface IMongoDbContext<T>
    {
        public IMongoCollection<T> GetCollection();
    }
}
