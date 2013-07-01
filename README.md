MongoDBIntIdGenerator
=====================

MongoDB Sequential integer Id Generator - Uses findAndModify to create sequential id's

Designed to provide sequential int IDs for documents, using the method outlined here: http://www.alexjamesbrown.com/blog/development/mongodb-incremental-ids/

Some unit tests are included, however this is not yet tested on scale, with replica sets etc...

**Usage**

    BsonClassMap.RegisterClassMap<MyClass>(cm => {
        cm.AutoMap();
        cm.IdMemberMap.SetIdGenerator(new IntId32Generator());
    });

	BsonClassMap.RegisterClassMap<MyClass>(cm => {
        cm.AutoMap();
        cm.IdMemberMap.SetIdGenerator(new IntId64Generator());
    });
