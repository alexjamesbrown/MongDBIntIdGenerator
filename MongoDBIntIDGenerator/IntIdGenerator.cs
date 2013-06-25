using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace MongoDBIntIdGenerator
{
    public class IntIdGenerator : IIdGenerator
    {
        public object GenerateId(object container, object document)
        {
            var idSequenceCollection = ((MongoCollection)container).Database.GetCollection("IDInt64Sequence2");

            var query = Query.EQ("_id", ((MongoCollection)container).Name);

            return idSequenceCollection
                .FindAndModify(query, null, Update.Inc("seq", 1L), true, true)
                .ModifiedDocument["seq"]
                .AsInt64;
        }

        public bool IsEmpty(object id)
        {
            return (long)id == 0;
        }
    }
}
