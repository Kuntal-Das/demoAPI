using Microsoft.AspNetCore.Mvc;
using Catalog.Api.Repositories;
using System.Collections.Generic;
using Catalog.Api.Entities;
using System;
using System.Linq;
using Catalog.Api.Dtos;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Catalog.Api.Controllers
{
    // Get /items
    [ApiController]
    [Route("items")]
    // [Route("[contoller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository repository;
        private readonly ILogger<ItemsController> logger;


        public ItemsController(IItemsRepository repository, ILogger<ItemsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // Get /items
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            var items = (await repository.GetItemsAsync())
                            .Select(item => item.AsDto());
            return items;
        }

        // GET /items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await repository.GetItemAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        // POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto CreatedItemDto)
        {
            Item CreatedItem = new()
            {
                Id = Guid.NewGuid(),
                Name = CreatedItemDto.Name,
                Price = CreatedItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await repository.CreateItemAsync(CreatedItem);

            return CreatedAtAction(nameof(GetItemAsync), new { ID = CreatedItem.Id }, CreatedItem.AsDto());
        }

        // PUT /items/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto UpdatedItemDto)
        {
            Item CurrentItem = await repository.GetItemAsync(id);
            if (CurrentItem == null)
            {
                return NotFound();
            }

            Item UpdatedItem = CurrentItem with
            {
                Name = UpdatedItemDto.Name,
                Price = UpdatedItemDto.Price
            };
            await repository.UpdateItemAsync(UpdatedItem);

            return NoContent();
        }

        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            Item ExistingItem = await repository.GetItemAsync(id);
            if (ExistingItem == null)
            {
                return NotFound();
            }

            await repository.DeleteItemAsync(id);
            return NoContent();
        }
    }
}