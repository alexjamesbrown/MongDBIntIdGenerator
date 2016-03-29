using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using NUnit.Framework;
using MongoDBIntIdGenerator.Tests.Stubs;
using MongoDB.Bson;

namespace MongoDBIntIdGenerator.Tests
{
    [TestFixture]
    public class Int32IdGeneratorTest
    {
        private IMongoDatabase _db;
        private MongoClient _client;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _client = new MongoClient("mongodb://localhost/?safe=true");
            _db = _client.GetDatabase("test");

			// Keep the collection clear for the tests.
			_db.GetCollection<BsonDocument> ("IdInt32").DeleteMany(new BsonDocument());
        }

        [SetUp]
        public void SetUp()
        {
            _client.DropDatabase("test");
        }

        [Test]
        public void Saving_Item_Has_Id_Of_1()
        {
            var item = new StubInt32Entity { Name = "Testing" };

            _db.GetCollection<StubInt32Entity>("testEntities").InsertOne(item);

            Assert.AreEqual(1, item.Id);
        }

        [Test]
        public void When_Saving_2_Items_Second_Item_Has_Id_Of_2()
        {
            var item1 = new StubInt32Entity { Name = "Testing" };
            var item2 = new StubInt32Entity { Name = "Testing 2" };

            _db.GetCollection<StubInt32Entity>("testEntities").InsertOne(item1);
            _db.GetCollection<StubInt32Entity>("testEntities").InsertOne(item2);

            Assert.AreEqual(2, item2.Id);
        }

        [Test]
        public void Save_Batch_Of_Items()
        {
            var items = new List<StubInt32Entity>();

            for (int i = 0; i < 1000; i++)
                items.Add(new StubInt32Entity { Name = "Item " + i });

            _db.GetCollection<StubInt32Entity>("testEntities").InsertMany(items);

            for (var i = 1; i < 1001; i++)
                Assert.That(items.Select(x => x.Id).Contains(i));
        }
    }
}
