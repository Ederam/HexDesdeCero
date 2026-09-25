using FluentValidation;

namespace Tienda.Application.Orders.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("El ID del cliente es obligatorio.")
            .MaximumLength(20).WithMessage("El ID del cliente no debe superar los 20 caracteres.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("La orden debe contener al menos un ítem.");

        RuleForEach(x => x.Items).ChildRules(items =>
        {
            items.RuleFor(i => i.ProductId)
                .NotEmpty().WithMessage("El ID del producto es obligatorio.");

            items.RuleFor(i => i.ProductName)
                .NotEmpty().WithMessage("El nombre del producto es obligatorio.");

            items.RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0.");

            items.RuleFor(i => i.UnitPrice)
                .GreaterThan(0).WithMessage("El precio unitario debe ser mayor a 0.");

            items.RuleFor(i => i.AvailableStock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock disponible no puede ser negativo.");
        });
    }
}