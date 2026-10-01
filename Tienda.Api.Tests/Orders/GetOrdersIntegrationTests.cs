using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Tienda.Application.Common.Models;
using Tienda.Application.Orders.Queries.GetOrderById;
using Xunit;

namespace Tienda.Api.Tests.Orders;

/// <summary>
/// Pruebas de integración de extremo a extremo (E2E) para el endpoint paginado de órdenes GET /api/orders.
/// </summary>
public class GetOrdersIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    /// <summary>
    /// Inicializa la fábrica de la aplicación web en memoria para ejecutar solicitudes de integración HTTP.
    /// </summary>
    /// <param name="factory">Fábrica suministrada por xUnit para hospedar el servidor de pruebas.</param>
    public GetOrdersIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    /// <summary>
    /// Verifica que al invocar el endpoint GET /api/orders con parámetros de paginación válidos, 
    /// se obtenga un código de estado 200 OK y la estructura paginada correspondiente.
    /// </summary>
    [Fact]
    public async Task GetOrders_ShouldReturnOkAndPagedResult_WhenQueryIsValid()
    {
        // Act
        HttpResponseMessage response = await _client.GetAsync("/api/orders?pageNumber=1&pageSize=5");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        PagedResult<OrderResponse>? result = await response.Content.ReadFromJsonAsync<PagedResult<OrderResponse>>();

        Assert.NotNull(result);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(5, result.PageSize);
        Assert.NotNull(result.Items);
    }

    /// <summary>
    /// Verifica que al enviar un número de página inválido (menor a 1), 
    /// el pipeline de validación responda con un estado 400 Bad Request.
    /// </summary>
    [Fact]
    public async Task GetOrders_ShouldReturnBadRequest_WhenPageNumberIsInvalid()
    {
        // Act
        HttpResponseMessage response = await _client.GetAsync("/api/orders?pageNumber=0&pageSize=10");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}