using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using System;

namespace MongoDBIntIdGenerator
{
	/// <summary>
	/// Int32 identifier generator.
	/// </summary>
    public class Int32IdGenerator : IntIdGeneratorBase
    {
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="MongoDBIntIdGenerator.Int32IdGenerator"/> class.
		/// </summary>
		/// <param name="idCollectionName">Identifier collection name.</param>
		public Int32IdGenerator(string idCollectionName) : base(idCollectionName)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MongoDBIntIdGenerator.Int32IdGenerator"/> class.
		/// </summary>
		public Int32IdGenerator() : base("IdInt32")
		{
		}
		#endregion

		#region implemented abstract members of IntIdGeneratorBase
		/// <summary>
		/// Creates the update builder.
		/// </summary>
		/// <returns>The update builder.</returns>
		protected override UpdateBuilder CreateUpdateBuilder ()
		{
			return Update.Inc ("seq", 1); 
		}

		/// <summary>
		/// Converts to int.
		/// </summary>
		/// <returns>The to int.</returns>
		/// <param name="value">Value.</param>
		protected override object ConvertToInt (BsonValue value)
		{
			return value.AsInt32;
		}

		/// <summary>
		/// Tests whether an Id is empty.
		/// </summary>
		/// <param name="id">The Id.</param>
		/// <returns>True if the Id is empty.</returns>
		public override bool IsEmpty (object id)
		{
			return (Int32)id == 0;
		}
		#endregion
    }


    /// <summary>
    /// Int32 identifier generator.
    /// </summary>
    public class Int32IdGenerator<TDocument> : IntIdGeneratorBase<TDocument>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDBIntIdGenerator.Int32IdGenerator"/> class.
        /// </summary>
        /// <param name="idCollectionName">Identifier collection name.</param>
        public Int32IdGenerator(string idCollectionName) : base(idCollectionName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDBIntIdGenerator.Int32IdGenerator"/> class.
        /// </summary>
        public Int32IdGenerator() : base("IdInt32")
        {
        }
        #endregion

        #region implemented abstract members of IntIdGeneratorBase
        /// <summary>
        /// Creates the update builder.
        /// </summary>
        /// <returns>The update builder.</returns>
        protected override UpdateDefinition<BsonDocument> CreateUpdateDefinition()
        {
            return Builders<BsonDocument>.Update.Inc(x=>x["seq"], 1);
        }

        /// <summary>
        /// Converts to int.
        /// </summary>
        /// <returns>The to int.</returns>
        /// <param name="value">Value.</param>
        protected override object ConvertToInt(BsonValue value)
        {
            return value.AsInt32;
        }

        /// <summary>
        /// Tests whether an Id is empty.
        /// </summary>
        /// <param name="id">The Id.</param>
        /// <returns>True if the Id is empty.</returns>
        public override bool IsEmpty(object id)
        {
            return (Int32)id == 0;
        }
        #endregion
    }
}
