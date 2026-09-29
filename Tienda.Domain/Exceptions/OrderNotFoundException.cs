namespace Tienda.Domain.Exceptions;

/// <summary>
/// Excepción lanzada cuando una orden solicitada no existe en el sistema de persistencia.
/// </summary>
public class OrderNotFoundException : Exception
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="OrderNotFoundException"/> con el identificador de la orden no encontrada.
    /// </summary>
    /// <param name="orderId">Identificador único (GUID) de la orden que no se encontró.</param>
    public OrderNotFoundException(Guid orderId)
        : base($"La orden con el ID '{orderId}' no fue encontrada.")
    {
    }
}