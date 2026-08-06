using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.Product;
using Application.Interfaces;
using Application.Mapping;
using Domain.Entities;

namespace Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        return product?.ToDto();
    }

    public async Task<IEnumerable<ProductDto>> GetPagedProductsAsync(int pageNumber, int pageSize)
    {
        var products = await _unitOfWork.Products.GetPagedProductsAsync(pageNumber, pageSize);
        return products.Select(p => p.ToDto());
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto dto, string createdBy)
    {
        var product = new Product
        {
            ProductName = dto.ProductName,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.CompleteAsync();

        return product.ToDto();
    }

    public async Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto dto, string modifiedBy)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
            return null;

        product.ProductName = dto.ProductName;
        product.ModifiedBy = modifiedBy;
        product.ModifiedOn = DateTime.UtcNow;

        _unitOfWork.Products.Update(product);
        await _unitOfWork.CompleteAsync();

        return product.ToDto();
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
            return false;

        _unitOfWork.Products.Delete(product);
        await _unitOfWork.CompleteAsync();
        return true;
    }
}
