using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Tienda.Api.Middlewares;
using Tienda.Application.Common.Behaviors;
using Tienda.Application.Orders.CreateOrder;
using Tienda.Domain.Ports;
using Tienda.Infrastructure.Persistence;
using Tienda.Infrastructure.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// 1. REGISTRO DE SERVICIOS
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Tienda API",
        Version = "v1",
        Description = "API de Backend bajo Arquitectura Hexagonal, CQRS y MediatR."
    });
});

// Registrar Infraestructura
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("TiendaDb"));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Registrar MediatR y FluentValidation desde el ensamblado de Application
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(CreateOrderCommandValidator).Assembly);

// 2. PIPELINE DE PETICIONES HTTP
WebApplication app = builder.Build();

// Registrar Middleware Global de Excepciones
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tienda API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();