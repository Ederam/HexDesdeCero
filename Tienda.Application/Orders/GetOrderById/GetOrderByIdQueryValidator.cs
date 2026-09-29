using FluentValidation;

namespace Tienda.Application.Orders.GetOrderById;

public class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
{
    public GetOrderByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("El ID de la orden es obligatorio y debe ser un GUID válido.");
    }
}