using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq.Expressions;

namespace MongoDBIntIdGenerator.Tests
{
    public static class MongoCollectionExtensions
    {
        public static void Insert<TEntity>(this IMongoCollection<TEntity> collection, TEntity entity)
        {
            collection.InsertOne(entity);
        }

        public static void RemoveAll<TEntity>(this IMongoCollection<TEntity> collection)
        {
            collection.DeleteMany(_ => true);
        }

        public static IMongoCollection<BsonDocument> GetCollection(this IMongoDatabase db, string name)
        {
            return db.GetCollection<BsonDocument>(name);
        }
    }
}
