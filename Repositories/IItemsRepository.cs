using System;
using System.Collections.Generic;
using Catalog.Entities;

namespace Catalog.Repositories
{
    public interface IItemsRepository
    {
        IEnumerable<Item> GetItems();
        Item GetItem(Guid id);

        void CreateItem(Item CreatedItem);
        void UpdateItem(Item UpdatedIitem);
        void DeleteItem(Guid id);
    }
}