using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Tienda.Application.UseCases;
using Tienda.Domain.Ports;
using Tienda.Infrastructure.Persistence;
using Tienda.Infrastructure.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// 1. REGISTRO DE SERVICIOS (Contenedor de Inyección de Dependencias)

// Habilitar soporte para Controladores
builder.Services.AddControllers();

// Configuración de OpenAPI / Swagger con metadata explícita
builder.Services.AddEndpointsApiExplorer();
// Configuración explícita del documento Swagger v1
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Tienda API",
        Version = "v1",
        Description = "API de Backend construida bajo Arquitectura Hexagonal"
    });
});

// Registrar Infraestructura (Base de datos e Implementaciones concretas)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("TiendaDb"));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Registrar Aplicación (Casos de Uso / Orquestación)
builder.Services.AddScoped<CreateOrderUseCase>();


// 2. CONSTRUCCIÓN Y PIPELINE DE PETICIONES HTTP
WebApplication app = builder.Build();

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

// Mapear la ruta de los Controladores
app.MapControllers();

app.Run();