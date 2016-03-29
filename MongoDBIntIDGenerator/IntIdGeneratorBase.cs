using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson.Serialization;

namespace MongoDBIntIdGenerator
{
    /// <summary>
    /// Base class for id generator based on integer values.
    /// </summary>
    public abstract class IntIdGeneratorBase : IIdGenerator
    {
        #region Fields
        private string m_idCollectionName;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDBIntIdGenerator.IntIdGeneratorBase"/> class.
        /// </summary>
        /// <param name="idCollectionName">Identifier collection name.</param>
        protected IntIdGeneratorBase(string idCollectionName)
        {
            m_idCollectionName = idCollectionName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDBIntIdGenerator.IntIdGeneratorBase"/> class.
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
        protected abstract UpdateBuilder CreateUpdateBuilder();

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
            var idSequenceCollection = ((MongoCollection)container).Database
                .GetCollection(m_idCollectionName);

            var query = Query.EQ("_id", ((MongoCollection)container).Name);

            return ConvertToInt(idSequenceCollection.FindAndModify(new FindAndModifyArgs()
            {
                Query = query,
                Update = CreateUpdateBuilder(),
                VersionReturned = FindAndModifyDocumentVersion.Modified,
                Upsert = true
            }).ModifiedDocument["seq"]);
        }
        #endregion
    }

    /// <summary>
    /// Base class for id generator based on integer values.
    /// </summary>
    public abstract class IntIdGeneratorBase<TDocument> : IIdGenerator
    {
        #region Fields
        private string m_idCollectionName;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDBIntIdGenerator.IntIdGeneratorBase"/> class.
        /// </summary>
        /// <param name="idCollectionName">Identifier collection name.</param>
        protected IntIdGeneratorBase(string idCollectionName)
        {
            m_idCollectionName = idCollectionName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDBIntIdGenerator.IntIdGeneratorBase"/> class.
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
        protected abstract UpdateDefinition<BsonDocument> CreateUpdateDefinition();

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
            var collection = ((IMongoCollection<TDocument>)container);
            var idSequenceCollection = collection.Database
                .GetCollection<BsonDocument>(m_idCollectionName);

            var query = Builders<BsonDocument>.Filter.Eq("_id", collection.CollectionNamespace.CollectionName);

            return ConvertToInt(idSequenceCollection.FindOneAndUpdate(
                query,
                CreateUpdateDefinition(),
                new FindOneAndUpdateOptions<BsonDocument>()
                {
                    IsUpsert = true,
                    ReturnDocument = ReturnDocument.After
                })["seq"]);
        }
        #endregion
    }
}

