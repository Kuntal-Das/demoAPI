using System;
using System.Collections.Generic;
using Catalog.Entities;
using MongoDB.Driver;

namespace Catalog.Repositories
{
    public class MongoDbItemsRepository : IItemsRepository
    {
        private readonly IMongoCollection<Item> itemsCollection;
        private const string DBName = "catalog";
        private const string CollectionName = "items";

        public MongoDbItemsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase db = mongoClient.GetDatabase(DBName);
            this.itemsCollection = db.GetCollection<Item>(CollectionName);
        }
        public void CreateItem(Item item)
        {
            itemsCollection.InsertOne(item);
        }

        public void DeleteItem(Guid id)
        {
            throw new NotImplementedException();
        }

        public Item GetItem(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Item> GetItems()
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(Item item)
        {
            throw new NotImplementedException();
        }
    }
}