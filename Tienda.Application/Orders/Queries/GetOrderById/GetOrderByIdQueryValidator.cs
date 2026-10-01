// File: Tienda.Application/Orders/Queries/GetOrderById/GetOrderByIdQueryValidator.cs
using FluentValidation;

namespace Tienda.Application.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
{
    public GetOrderByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El ID de la orden es obligatorio.");
    }
}