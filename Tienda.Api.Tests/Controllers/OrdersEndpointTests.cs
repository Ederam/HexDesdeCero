using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Tienda.Application.Orders.Commands.CreateOrder;
using Xunit;

namespace Tienda.Api.Tests.Controllers;

public class OrdersEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OrdersEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateOrder_ShouldReturnCreated_WhenPayloadIsValid()
    {
        // Arrange
        CreateOrderCommand command = new CreateOrderCommand(
            CustomerId: "Camilo Ramírez",
            Items: new List<CreateOrderItemCommand>
            {
                new CreateOrderItemCommand(Guid.NewGuid(), "Producto Test", 2, 150.0m)
            }
        );

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/orders", command);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        CreateOrderResponse? orderResponse = await response.Content.ReadFromJsonAsync<CreateOrderResponse>();
        Assert.NotNull(orderResponse);
        Assert.NotEqual(Guid.Empty, orderResponse.Id);
    }
}