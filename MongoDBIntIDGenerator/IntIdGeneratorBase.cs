using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MongoDBIntIdGenerator
{
    /// <summary>
    /// Base class for id generator based on integer values.
    /// </summary>
    public abstract class IntIdGeneratorBase<T> : IIdGenerator where T : class
    {
        #region Fields
        private string m_idCollectionName;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="idCollectionName">Identifier collection name.</param>
        protected IntIdGeneratorBase(string idCollectionName)
        {
            m_idCollectionName = idCollectionName;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected IntIdGeneratorBase() : this("IDs")
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates the update builder.
        /// </summary>
        /// <returns>The update builder.</returns>
        protected abstract UpdateDefinition<BsonDocument> CreateUpdateBuilder();

        /// <summary>
        /// Converts to int.
        /// </summary>
        /// <returns>The to int.</returns>
        /// <param name="value">Value.</param>
        protected abstract object ConvertToInt(BsonValue value);

        /// <summary>
        /// Tests whether an Id is empty.
        /// </summary>
        /// <param name="id">The Id.</param>
        /// <returns>True if the Id is empty.</returns>
        public abstract bool IsEmpty(object id);

        /// <summary>
        /// Generates an Id for a document.
        /// </summary>
        /// <param name="container">The container of the document (will be a MongoCollection when called from the C# driver).</param>
        /// <param name="document">The document.</param>
        /// <returns>An Id.</returns>
        public object GenerateId(object container, object document)
        {
            var idSequenceCollection = ((IMongoCollection<T>)container).Database
                .GetCollection<BsonDocument>(m_idCollectionName);

            var collectionName = document.GetType().Name;

            var filterQuery = Builders<BsonDocument>.Filter.Eq("_id", collectionName);
            var updates = Builders<BsonDocument>.Update.Inc("seq", 1);
            var updateOptions = new FindOneAndUpdateOptions<BsonDocument>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };

            var doc = idSequenceCollection.FindOneAndUpdate(filterQuery, updates, updateOptions);

            return ConvertToInt(doc["seq"]);
        }
        #endregion
    }
}

