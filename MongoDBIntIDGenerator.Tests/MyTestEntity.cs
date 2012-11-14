using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBIntIdGenerator.Tests
{
    public class MyTestEntity
    {
        [BsonId(IdGenerator = typeof(IntIdGenerator))]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}