using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Repositories
{
    public class MongoDbItemsRepository : IItemsRepository
    {
        private readonly IMongoCollection<Item> itemsCollection;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;
        private const string DBName = "catalog";
        private const string CollectionName = "items";

        public MongoDbItemsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase db = mongoClient.GetDatabase(DBName);
            this.itemsCollection = db.GetCollection<Item>(CollectionName);
        }
        public void CreateItem(Item CreatedItem)
        {
            itemsCollection.InsertOne(CreatedItem);
        }

        public void DeleteItem(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(item => item.Id, id);
            itemsCollection.DeleteOne(filter);
        }

        public Item GetItem(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(item => item.Id, id);
            return itemsCollection.Find(filter).SingleOrDefault();
        }

        public IEnumerable<Item> GetItems()
        {
            return itemsCollection.Find(new BsonDocument()).ToList();
        }

        public void UpdateItem(Item UpdatedIitem)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(item => item.Id, UpdatedIitem.Id);
            itemsCollection.ReplaceOne(filter, UpdatedIitem);
        }
    }
}