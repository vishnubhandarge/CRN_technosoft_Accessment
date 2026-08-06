using Application.DTOs.Product;
using Application.Validators;
using Xunit;

namespace Application.Tests;

public class CreateProductDtoValidatorTests
{
    private readonly CreateProductDtoValidator _validator;

    public CreateProductDtoValidatorTests()
    {
        _validator = new CreateProductDtoValidator();
    }

    [Fact]
    public void Validator_WithValidName_ShouldBeValid()
    {
        // Arrange
        var model = new CreateProductDto { ProductName = "Valid Product" };

        // Act
        var result = _validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validator_WithEmptyName_ShouldHaveValidationError()
    {
        // Arrange
        var model = new CreateProductDto { ProductName = "" };

        // Act
        var result = _validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "ProductName" && e.ErrorMessage.Contains("required"));
    }
}
