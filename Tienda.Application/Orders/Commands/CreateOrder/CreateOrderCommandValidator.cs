using FluentValidation;

namespace Tienda.Application.Orders.Commands.CreateOrder;

/// <summary>
/// Validador de reglas de negocio de entrada para el comando <see cref="CreateOrderCommand"/>.
/// Intercepta y valida los datos antes de ejecutar el Handler.
/// </summary>
public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    /// <summary>
    /// Inicializa las reglas de validación para el identificador del cliente y sus ítems.
    /// </summary>
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("El identificador del cliente es obligatorio.")
            .MaximumLength(100).WithMessage("El identificador del cliente no debe superar los 100 caracteres.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("La orden debe incluir al menos un producto.");

        RuleForEach(x => x.Items).ChildRules(items =>
        {
            items.RuleFor(i => i.ProductId)
                .NotEmpty().WithMessage("El identificador del producto es obligatorio.");

            items.RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("La cantidad debe ser mayor a cero.");

            items.RuleFor(i => i.UnitPrice)
                .GreaterThan(0).WithMessage("El precio unitario debe ser mayor a cero.");
        });
    }
}