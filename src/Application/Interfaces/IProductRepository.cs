using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IEnumerable<Product>> GetPagedProductsAsync(int pageNumber, int pageSize);
    Task<Product?> GetProductWithItemsAsync(int id);
}
