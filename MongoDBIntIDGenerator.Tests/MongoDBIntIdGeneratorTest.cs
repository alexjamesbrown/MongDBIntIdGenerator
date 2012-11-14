using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using NUnit.Framework;

namespace MongoDBIntIdGenerator.Tests
{
    [TestFixture]
    public class MongoDBIntIdGeneratorTest
    {
        private MongoDatabase _db;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _db = MongoServer
                .Create("mongodb://localhost/?safe=true")
                .GetDatabase("test");
        }

        [SetUp]
        public void SetUp()
        {
            _db.Drop();
        }

        [Test]
        public void Saving_Item_Has_Id_Of_1()
        {
            var item = new MyTestEntity { Name = "Testing" };

            _db.GetCollection<MyTestEntity>("testEntities").Save(item);

            Assert.AreEqual(1, item.Id);
        }

        [Test]
        public void When_Saving_2_Items_Second_Item_Has_Id_Of_2()
        {
            var item1 = new MyTestEntity { Name = "Testing" };
            var item2 = new MyTestEntity { Name = "Testing 2" };

            _db.GetCollection<MyTestEntity>("testEntities").Save(item1);
            _db.GetCollection<MyTestEntity>("testEntities").Save(item2);

            Assert.AreEqual(2, item2.Id);
        }

        [Test]
        public void Save_Batch_Of_Items()
        {
            var items = new List<MyTestEntity>();

            for (int i = 0; i < 1000; i++)
                items.Add(new MyTestEntity { Name = "Item " + i });

            _db.GetCollection<MyTestEntity>("testEntities").InsertBatch(items);

            for (var i = 1; i < 1001; i++)
                Assert.That(items.Select(x => x.Id).Contains(i));
        }
    }
}
