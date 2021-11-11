using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Api.Controllers;
using Catalog.Api.Dtos;
using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Catalog.UnitTests
{
    public class ItemsControllerTests
    {
        private readonly Mock<IItemsRepository> repositoryStub = new();
        private readonly Mock<ILogger<ItemsController>> loggerStub = new();
        private readonly Random rand = new();

        [Fact]
        // public void UnitOfWork_StateUnderTest_ExpectedBehavious()
        public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()
        {
            // Arrange
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Item)null);

            ItemsController controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.GetItemAsync(Guid.NewGuid());

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();

            // Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()
        {
            // Arrange
            Item expectedItem = CreateRandomItem();

            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedItem);
            ItemsController controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.GetItemAsync(Guid.NewGuid());

            // Assert
            result.Value.Should().BeEquivalentTo(expectedItem);

            // Assert.IsType<ItemDto>(result.Value);

            // ItemDto dto = (result as ActionResult<ItemDto>).Value;
            // Assert.Equal(expectedItem.Id, dto.Id);
            // Assert.Equal(expectedItem.Name, dto.Name);
            // Assert.Equal(expectedItem.Price, dto.Price);
            // Assert.Equal(expectedItem.CreatedDate, dto.CreatedDate);
        }

        [Fact]
        public async Task GetItemsAsync_WithExistingItems_ReturnsAllItem()
        {
            // Arrange
            Item[] expectedItems = new[] {
                CreateRandomItem(),
                CreateRandomItem(),
                CreateRandomItem()
            };

            repositoryStub.Setup(repo => repo.GetItemsAsync())
                .ReturnsAsync(expectedItems);

            ItemsController controller = new(repositoryStub.Object, loggerStub.Object);

            // Act
            IEnumerable<ItemDto> actualItems = await controller.GetItemsAsync();

            // Assert
            actualItems.Should().BeEquivalentTo(expectedItems);
        }

        [Fact]
        public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
        {
            // Arrange
            CreateItemDto itemToCreate = new CreateItemDto(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(), 
                rand.Next(1000));
                
            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.CreateItemAsync(itemToCreate);

            // Assert
            var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;
            itemToCreate.Should().BeEquivalentTo(
                createdItem,
                options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers()
            );

            createdItem.Id.Should().NotBeEmpty();
            createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        }



        [Fact]
        public async Task UpdatedItemAsync_WithItemToUpdate_ReturnsNoContent()
        {
            // Arrage
            Item existingItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingItem);

            Guid itemId = existingItem.Id;
            UpdateItemDto itemToUpdate = new(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                existingItem.Price + 3
            );

            ItemsController controller = new(repositoryStub.Object, loggerStub.Object);

            // Act 
            var result = await controller.UpdateItemAsync(itemId, itemToUpdate);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteItemAsync_WithItemToDelete_ReturnsNoContent()
        {
            Item existingItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingItem);

            ItemsController controller = new(repositoryStub.Object, loggerStub.Object);

            // Act 
            var result = await controller.DeleteItemAsync(existingItem.Id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
        
        private Item CreateRandomItem()
        {
            return new Item()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Price = rand.Next(1000),
                CreatedDate = DateTimeOffset.UtcNow
            };
        }
    }
}
