using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.Item;
using Application.Interfaces;
using Application.Mapping;
using Domain.Entities;

namespace Application.Services;

public class ItemService : IItemService
{
    private readonly IUnitOfWork _unitOfWork;

    public ItemService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ItemDto?> GetItemByIdAsync(int id)
    {
        var item = await _unitOfWork.Items.GetByIdAsync(id);
        return item?.ToDto();
    }

    public async Task<IEnumerable<ItemDto>> GetItemsByProductIdAsync(int productId)
    {
        var items = await _unitOfWork.Items.GetItemsByProductIdAsync(productId);
        return items.Select(i => i.ToDto());
    }

    public async Task<ItemDto> CreateItemAsync(CreateItemDto dto)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
        if (product == null)
        {
            throw new System.ArgumentException($"Product with ID {dto.ProductId} does not exist.");
        }

        var item = new Item
        {
            ProductId = dto.ProductId,
            Quantity = dto.Quantity
        };

        await _unitOfWork.Items.AddAsync(item);
        await _unitOfWork.CompleteAsync();

        return item.ToDto();
    }

    public async Task<ItemDto?> UpdateItemAsync(int id, UpdateItemDto dto)
    {
        var item = await _unitOfWork.Items.GetByIdAsync(id);
        if (item == null)
            return null;

        item.Quantity = dto.Quantity;

        _unitOfWork.Items.Update(item);
        await _unitOfWork.CompleteAsync();

        return item.ToDto();
    }

    public async Task<bool> DeleteItemAsync(int id)
    {
        var item = await _unitOfWork.Items.GetByIdAsync(id);
        if (item == null)
            return false;

        _unitOfWork.Items.Delete(item);
        await _unitOfWork.CompleteAsync();
        return true;
    }
}
