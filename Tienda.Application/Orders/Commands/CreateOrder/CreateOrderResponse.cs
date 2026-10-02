namespace Tienda.Application.Orders.Commands.CreateOrder;

/// <summary>
/// DTO de salida enviado tras procesar exitosamente la creación de una orden.
/// </summary>
/// <param name="Id">Identificador único de la orden generada.</param>
/// <param name="CustomerName">Nombre del cliente registrado en la orden.</param>
/// <param name="TotalAmount">Monto total calculado de la orden.</param>
/// <param name="CreatedAt">Fecha y hora UTC en la que se registró la orden.</param>
public sealed record CreateOrderResponse(Guid Id);