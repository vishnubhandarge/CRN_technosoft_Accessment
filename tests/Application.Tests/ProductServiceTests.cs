using System;
using System.Threading.Tasks;
using Application.DTOs.Product;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Moq;
using Xunit;

namespace Application.Tests;

public class ProductServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductRepository> _mockProductRepo;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepo = new Mock<IProductRepository>();
        _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepo.Object);
        _productService = new ProductService(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task GetProductByIdAsync_WithValidId_ReturnsProductDto()
    {
        // Arrange
        var product = new Product { Id = 1, ProductName = "Test Product", CreatedBy = "Admin", CreatedOn = DateTime.UtcNow };
        _mockProductRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _productService.GetProductByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.ProductName, result.ProductName);
        Assert.Equal(product.Id, result.Id);
    }

    [Fact]
    public async Task CreateProductAsync_AddsProductAndCompletesUnitOfWork()
    {
        // Arrange
        var createDto = new CreateProductDto { ProductName = "New Product" };
        var createdBy = "User1";

        // Act
        var result = await _productService.CreateProductAsync(createDto, createdBy);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createDto.ProductName, result.ProductName);
        Assert.Equal(createdBy, result.CreatedBy);

        _mockProductRepo.Verify(r => r.AddAsync(It.Is<Product>(p => p.ProductName == createDto.ProductName && p.CreatedBy == createdBy)), Times.Once);
        _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
    }
}
