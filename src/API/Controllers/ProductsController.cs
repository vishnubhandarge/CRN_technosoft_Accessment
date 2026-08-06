using System.Security.Claims;
using System.Threading.Tasks;
using Application.DTOs.Product;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers;

// Controller for managing Products and their related Items.
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IItemService _itemService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, IItemService itemService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _itemService = itemService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Request received for paged products: Page {PageNumber}, Size {PageSize}", pageNumber, pageSize);

        if (pageNumber <= 0) pageNumber = 1;
        if (pageSize <= 0 || pageSize > 100) pageSize = 10;

        var products = await _productService.GetPagedProductsAsync(pageNumber, pageSize);
        return Ok(new { Success = true, Data = products });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("Request received to fetch product with ID: {Id}", id);

        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            _logger.LogWarning("Product with ID {Id} was not found.", id);
            return NotFound(new { Success = false, Message = $"Product with ID {id} not found." });
        }

        return Ok(new { Success = true, Data = product });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        var username = User.Identity?.Name ?? "System";
        _logger.LogInformation("User '{Username}' is creating product: '{ProductName}'", username, dto.ProductName);

        var product = await _productService.CreateProductAsync(dto, username);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, new { Success = true, Data = product });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
    {
        var username = User.Identity?.Name ?? "System";
        _logger.LogInformation("User '{Username}' is updating product ID {Id} to name '{ProductName}'", username, id, dto.ProductName);

        var product = await _productService.UpdateProductAsync(id, dto, username);
        if (product == null)
        {
            _logger.LogWarning("Product update failed. Product ID {Id} was not found.", id);
            return NotFound(new { Success = false, Message = $"Product with ID {id} not found." });
        }

        return Ok(new { Success = true, Data = product });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var username = User.Identity?.Name ?? "System";
        _logger.LogInformation("Admin user '{Username}' is deleting product ID {Id}", username, id);

        var result = await _productService.DeleteProductAsync(id);
        if (!result)
        {
            _logger.LogWarning("Product deletion failed. Product ID {Id} was not found.", id);
            return NotFound(new { Success = false, Message = $"Product with ID {id} not found." });
        }

        return Ok(new { Success = true, Message = "Product deleted successfully." });
    }

    [HttpGet("{id}/items")]
    public async Task<IActionResult> GetItems(int id)
    {
        _logger.LogInformation("Request received for items belonging to product ID {Id}", id);

        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            _logger.LogWarning("Fetch items failed. Product ID {Id} was not found.", id);
            return NotFound(new { Success = false, Message = $"Product with ID {id} not found." });
        }

        var items = await _itemService.GetItemsByProductIdAsync(id);
        return Ok(new { Success = true, Data = items });
    }
}
