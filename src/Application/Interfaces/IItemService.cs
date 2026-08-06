using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Item;

namespace Application.Interfaces;

public interface IItemService
{
    Task<ItemDto?> GetItemByIdAsync(int id);
    Task<IEnumerable<ItemDto>> GetItemsByProductIdAsync(int productId);
    Task<ItemDto> CreateItemAsync(CreateItemDto dto);
    Task<ItemDto?> UpdateItemAsync(int id, UpdateItemDto dto);
    Task<bool> DeleteItemAsync(int id);
}
