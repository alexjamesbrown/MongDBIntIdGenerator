using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDBIntIdGenerator.Tests
{
    public class MyTestEntity
    {
        [BsonId(IdGenerator = typeof(IntIdGenerator<BsonInt32>))]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}