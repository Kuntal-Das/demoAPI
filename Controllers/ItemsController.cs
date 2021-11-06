using Microsoft.AspNetCore.Mvc;
using Catalog.Repositories;
using System.Collections.Generic;
using Catalog.Entities;
using System;
using System.Linq;
using Catalog.Dtos;

namespace Catalog.Controllers
{
    // Get /items
    [ApiController]
    [Route("items")]
    // [Route("[contoller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository repository;

        public ItemsController(IItemsRepository repository)
        {
            this.repository = repository;
        }

        // Get /items
        [HttpGet]
        public IEnumerable<ItemDto> GetItems()
        {
            var items = repository.GetItems().Select(item => item.AsDto());
            return items;
        }

        // GET /items/{id}
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id)
        {
            var item = repository.GetItem(id);
            if (item == null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        // POST /items
        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto CreatedItemDto)
        {
            Item CreatedItem = new()
            {
                Id = Guid.NewGuid(),
                Name = CreatedItemDto.Name,
                Price = CreatedItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            repository.CreateItem(CreatedItem);

            return CreatedAtAction(nameof(GetItem), new { ID = CreatedItem.Id }, CreatedItem.AsDto());
        }

        // PUT /items/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDto UpdatedItemDto)
        {
            Item CurrentItem = repository.GetItem(id);
            if (CurrentItem == null)
            {
                return NotFound();
            }

            Item UpdatedItem = CurrentItem with
            {
                Name = UpdatedItemDto.Name,
                Price = UpdatedItemDto.Price
            };
            repository.UpdateItem(UpdatedItem);

            return NoContent();
        }

        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id)
        {
            Item ExistingItem = repository.GetItem(id);
            if (ExistingItem == null)
            {
                return NotFound();
            }

            repository.DeleteItem(id);
            return NoContent();
        }
    }
}