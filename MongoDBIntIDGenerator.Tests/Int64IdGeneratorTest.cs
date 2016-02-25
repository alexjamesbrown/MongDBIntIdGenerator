using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using NUnit.Framework;
using MongoDBIntIdGenerator.Tests.Stubs;

namespace MongoDBIntIdGenerator.Tests
{
    [TestFixture]
    public class Int64IdGeneratorTest
    {
        private IMongoClient _client;
        private IMongoDatabase _db;
        private const string database = "test";


        [OneTimeSetUp]
        public void TestFixtureSetUp()
        {
            _client = new MongoClient("mongodb://localhost/?safe=true");

            _db = _client.GetDatabase(database);

            // Keep the collection clear for the tests.
            _db.GetCollection("IdInt64").RemoveAll();
        }

        [SetUp]
        public void SetUp()
        {
            _client.DropDatabase(database);
        }

        [Test]
        public void Saving_Item_Has_Id_Of_1()
        {
            var item = new StubInt64Entity { Name = "Testing" };

            _db.GetCollection<StubInt64Entity>("testEntities").Insert(item);

            Assert.AreEqual(1, item.Id);
        }

        [Test]
        public void When_Saving_2_Items_Second_Item_Has_Id_Of_2()
        {
            var item1 = new StubInt64Entity { Name = "Testing" };
            var item2 = new StubInt64Entity { Name = "Testing 2" };

            _db.GetCollection<StubInt64Entity>("testEntities").Insert(item1);
            _db.GetCollection<StubInt64Entity>("testEntities").Insert(item2);

            Assert.AreEqual(2, item2.Id);
        }

        [Test]
        public void Save_Batch_Of_Items()
        {
            var items = new List<StubInt64Entity>();

            for (int i = 0; i < 1000; i++)
                items.Add(new StubInt64Entity { Name = "Item " + i });

            _db.GetCollection<StubInt64Entity>("testEntities").InsertMany(items);

            for (var i = 1; i < 1001; i++)
                Assert.That(items.Select(x => x.Id).Contains(i));
        }
    }
}
