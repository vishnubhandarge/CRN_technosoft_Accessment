using System;
using System.Threading.Tasks;
using Application.DTOs.Item;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ItemsController : ControllerBase
{
    private readonly IItemService _itemService;

    public ItemsController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _itemService.GetItemByIdAsync(id);
        if (item == null)
            return NotFound(new { Success = false, Message = $"Item with ID {id} not found." });

        return Ok(new { Success = true, Data = item });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateItemDto dto)
    {
        try
        {
            var item = await _itemService.CreateItemAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, new { Success = true, Data = item });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Success = false, Message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateItemDto dto)
    {
        var item = await _itemService.UpdateItemAsync(id, dto);
        if (item == null)
            return NotFound(new { Success = false, Message = $"Item with ID {id} not found." });

        return Ok(new { Success = true, Data = item });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _itemService.DeleteItemAsync(id);
        if (!result)
            return NotFound(new { Success = false, Message = $"Item with ID {id} not found." });

        return Ok(new { Success = true, Message = "Item deleted successfully." });
    }
}
