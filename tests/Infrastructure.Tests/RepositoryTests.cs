using System;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Infrastructure.Tests;

public class RepositoryTests
{
    private async Task<ApplicationDbContext> GetDbContextAsync()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    [Fact]
    public async Task ProductRepository_AddAsync_AddsProductToDatabase()
    {
        // Arrange
        using var context = await GetDbContextAsync();
        var repository = new ProductRepository(context);
        var product = new Product { ProductName = "Test Product", CreatedBy = "TestUser", CreatedOn = DateTime.UtcNow };

        // Act
        await repository.AddAsync(product);
        await context.SaveChangesAsync();

        // Assert
        var result = await context.Products.FirstOrDefaultAsync(p => p.ProductName == "Test Product");
        Assert.NotNull(result);
        Assert.Equal("TestUser", result.CreatedBy);
    }
}
