using FluentValidation.TestHelper;
using Tienda.Application.Orders.Commands.CreateOrder;
using Xunit;

namespace Tienda.Application.Tests.Orders.Commands;

/// <summary>
/// Pruebas unitarias para validar las reglas de <see cref="CreateOrderCommandValidator"/>.
/// </summary>
public class CreateOrderCommandValidatorTests
{
    private readonly CreateOrderCommandValidator _validator;

    public CreateOrderCommandValidatorTests()
    {
        _validator = new CreateOrderCommandValidator();
    }

    [Fact]
    public async Task Should_Have_Error_When_CustomerId_Is_Empty()
    {
        // Arrange
        Guid productId = Guid.NewGuid();
        CreateOrderCommand command = new CreateOrderCommand(
            CustomerId: string.Empty,
            Items: new List<CreateOrderItemCommand>
            {
                new CreateOrderItemCommand(productId, "Producto Test", 2, 100.0m)
            }
        );

        // Act
        TestValidationResult<CreateOrderCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.CustomerId);
    }

    [Fact]
    public async Task Should_Have_Error_When_Items_Collection_Is_Empty()
    {
        // Arrange
        CreateOrderCommand command = new CreateOrderCommand(
            CustomerId: "Camilo Ramírez",
            Items: new List<CreateOrderItemCommand>()
        );

        // Act
        TestValidationResult<CreateOrderCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Items);
    }

    [Fact]
    public async Task Should_Have_Error_When_Item_Quantity_Is_Zero_Or_Negative()
    {
        // Arrange
        Guid productId = Guid.NewGuid();
        CreateOrderCommand command = new CreateOrderCommand(
            CustomerId: "Camilo Ramírez",
            Items: new List<CreateOrderItemCommand>
            {
                new CreateOrderItemCommand(productId, "Producto Test", 0, 100.0m)
            }
        );

        // Act
        TestValidationResult<CreateOrderCommand> result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Items[0].Quantity");
    }
}