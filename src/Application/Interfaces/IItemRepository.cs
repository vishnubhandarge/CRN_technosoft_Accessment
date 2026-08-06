using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces;

public interface IItemRepository : IGenericRepository<Item>
{
    Task<IEnumerable<Item>> GetItemsByProductIdAsync(int productId);
}
