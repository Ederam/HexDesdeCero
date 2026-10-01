using FluentValidation;

namespace Tienda.Application.Orders.Queries.GetOrders;

/// <summary>
/// Validador de reglas de negocio para los parámetros de paginación en <see cref="GetOrdersQuery"/>.
/// </summary>
public class GetOrdersQueryValidator : AbstractValidator<GetOrdersQuery>
{
    /// <summary>
    /// Inicializa y define las reglas de validación aplicables a la consulta de órdenes.
    /// </summary>
    public GetOrdersQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("El número de página debe ser mayor o igual a 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("El tamaño de página debe estar entre 1 y 100.");
    }
}