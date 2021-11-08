using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Api.Entities;

namespace Catalog.Api.Repositories
{
    public interface IItemsRepository
    {
        Task<IEnumerable<Item>> GetItemsAsync();
        Task<Item> GetItemAsync(Guid id);

        Task CreateItemAsync(Item CreatedItem);
        Task UpdateItemAsync(Item UpdatedIitem);
        Task DeleteItemAsync(Guid id);
    }
}