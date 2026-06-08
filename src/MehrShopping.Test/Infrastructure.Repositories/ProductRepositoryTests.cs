using Xunit;
using Microsoft.EntityFrameworkCore;
using Moq;
using MehrShopping.Infrastructure.Repositories;
using MehrShopping.Infrastructure.Data;
using ProductEntity = MehrShopping.Domain.Entities.Product;

namespace MehrShopping.Test.Infrastructure.Repositories
{
    public class ProductRepositoryTests
    {
        private readonly ProductRepository _repository;
        private readonly DbContextOptions<MehrShoppingDbContext> _options;

        public ProductRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<MehrShoppingDbContext>()
                .UseInMemoryDatabase("TestDB")
                .Options;

            var context = new MehrShoppingDbContext(_options);
            _repository = new ProductRepository(context);
        }

        [Fact]
        public async Task AddAsync_ShouldPersistProduct()
        {

            var product = ProductEntity.Create("Test", 10);

            await _repository.AddAsync(product);

            var retrieved = await _repository.GetByIdAsync(product.Id);
            Assert.NotNull(retrieved);
            Assert.Equal(product.Name, retrieved.Name);
            Assert.Equal(product.StockQuantity.Value, retrieved.StockQuantity.Value);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct()
        {
            var product = ProductEntity.Create("TestProduct", 10);

            await _repository.AddAsync(product);

            var retrieved = await _repository.GetByIdAsync(product.Id);
            Assert.NotNull(retrieved);
            Assert.Equal(product.Name, retrieved.Name);
            Assert.Equal(product.StockQuantity.Value, retrieved.StockQuantity.Value);
        }

        [Fact]
        public async Task Update_ShouldUpdateProduct()
        {
            var product = ProductEntity.Create("TestProduct", 10);

            await _repository.AddAsync(product);

            product.DecreaseStock(5);
            _repository.Update(product);

            var updated = await _repository.GetByIdAsync(product.Id);
            Assert.Equal(5, updated.StockQuantity.Value);
        }

        [Fact]
        public async Task Delete_ShouldRemoveProduct()
        {
            var product = ProductEntity.Create("TestProduct", 10);

            await _repository.AddAsync(product);

            _repository.Delete(product);

            var retrieved = await _repository.GetByIdAsync(product.Id);
            Assert.Null(retrieved);
        }
    }
}

