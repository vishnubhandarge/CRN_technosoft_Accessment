using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Product;

namespace Application.Interfaces;

public interface IProductService
{
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<IEnumerable<ProductDto>> GetPagedProductsAsync(int pageNumber, int pageSize);
    Task<ProductDto> CreateProductAsync(CreateProductDto dto, string createdBy);
    Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto dto, string modifiedBy);
    Task<bool> DeleteProductAsync(int id);
}
