using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using NUnit.Framework;
using MongoDBIntIdGenerator.Tests.Stubs;

namespace MongoDBIntIdGenerator.Tests
{
    [TestFixture]
    public class Int32IdGeneratorTest
    {
        private MongoDatabase _db;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
			_db = new MongoClient("mongodb://localhost/?safe=true")
                .GetServer()
                .GetDatabase("test");

			// Keep the collection clear for the tests.
			_db.GetCollection ("IdInt32").RemoveAll ();
        }

        [SetUp]
        public void SetUp()
        {
            _db.Drop();
        }

        [Test]
        public void Saving_Item_Has_Id_Of_1()
        {
            var item = new StubInt32Entity { Name = "Testing" };

            _db.GetCollection<StubInt32Entity>("testEntities").Save(item);

            Assert.AreEqual(1, item.Id);
        }

        [Test]
        public void When_Saving_2_Items_Second_Item_Has_Id_Of_2()
        {
            var item1 = new StubInt32Entity { Name = "Testing" };
            var item2 = new StubInt32Entity { Name = "Testing 2" };

            _db.GetCollection<StubInt32Entity>("testEntities").Save(item1);
            _db.GetCollection<StubInt32Entity>("testEntities").Save(item2);

            Assert.AreEqual(2, item2.Id);
        }

        [Test]
        public void Save_Batch_Of_Items()
        {
            var items = new List<StubInt32Entity>();

            for (int i = 0; i < 1000; i++)
                items.Add(new StubInt32Entity { Name = "Item " + i });

            _db.GetCollection<StubInt32Entity>("testEntities").InsertBatch(items);

            for (var i = 1; i < 1001; i++)
                Assert.That(items.Select(x => x.Id).Contains(i));
        }
    }
}
