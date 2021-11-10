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
            ActionResult<ItemDto> result = await controller.GetItemAsync(Guid.NewGuid());

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();

            // Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()
        {
            // Arrange
            Item expectedItem = CrreateRandomItem();

            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedItem);
            ItemsController controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            ActionResult<ItemDto> result = await controller.GetItemAsync(Guid.NewGuid());

            // Assert
            result.Value.Should().BeEquivalentTo(
                expectedItem,
                options => options.ComparingByMembers<Item>()
                );

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
                CrreateRandomItem(),
                CrreateRandomItem(),
                CrreateRandomItem()
            };

            repositoryStub.Setup(repo => repo.GetItemsAsync())
                .ReturnsAsync(expectedItems);

            ItemsController controller = new(repositoryStub.Object, loggerStub.Object);

            // Act
            IEnumerable<ItemDto> actualItems = await controller.GetItemsAsync();

            // Assert
            actualItems.Should().BeEquivalentTo(
                expectedItems,
                options => options.ComparingByMembers<Item>()
            );
        }

        [Fact]
        public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
        {
            // Arrange
            CreateItemDto itemToCreate = new CreateItemDto()
            {
                Name = Guid.NewGuid().ToString(),
                Price = rand.Next(1000)
            };

            ItemsController controller = new(repositoryStub.Object, loggerStub.Object);

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
            // TODO
            throw new NotImplementedException("TODO: UpdatedItemAsync_WithItemToUpdate_ReturnsNoContent");
            // Arrage

            // Act 

            // Assert
        }


        private Item CrreateRandomItem()
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
