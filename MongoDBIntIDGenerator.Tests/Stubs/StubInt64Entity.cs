using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDBIntIdGenerator.Tests.Stubs
{
	public class StubInt64Entity
	{
		[BsonId(IdGenerator = typeof(Int64IdGenerator<StubInt64Entity>))]
		public long Id { get; set; }
		public string Name { get; set; }
	}
}