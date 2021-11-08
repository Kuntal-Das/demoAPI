using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Api.Repositories
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
        public async Task CreateItemAsync(Item CreatedItem)
        {
            await itemsCollection.InsertOneAsync(CreatedItem);
        }

        public async Task DeleteItemAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(item => item.Id, id);
            await itemsCollection.DeleteOneAsync(filter);
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(item => item.Id, id);
            return await itemsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await itemsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateItemAsync(Item UpdatedIitem)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(item => item.Id, UpdatedIitem.Id);
            await itemsCollection.ReplaceOneAsync(filter, UpdatedIitem);
        }
    }
}