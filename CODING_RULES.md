🛡️ Reglas de Backend (.NET 8 & Arquitectura Hexagonal)
Aislamiento de Dominio: Tienda.Domain no puede tener ninguna referencia a librerías externas de terceros (salvo utilidades básicas de .NET) ni a otros proyectos.

CQRS con MediatR:

Commands: Modifican estado. Se definen como record inmutables.

Queries: Solo lectura. Retornan DTOs inmutables (record).

Handlers: Un handler por archivo, sufijo Handler, viviendo en Tienda.Application.

Inmutabilidad y DTOs: Todos los DTOs, Requests y Responses se definen usando public record.

Documentación XML: Toda interfaz, clase pública, DTO y controlador debe llevar sus comentarios XML (<summary>, <param>, <returns>).

Manejo de Excepciones: No usar bloques try-catch dispersos en controladores. Las reglas de negocio lanzan excepciones de dominio (ej. OrderNotFoundException) y se capturan en un Middleware / ExceptionHandler global.

🧪 Reglas de Pruebas (xUnit + NSubstitute + NetArchTest)
Estructura AAA: Formato explícito obligado en cada método de prueba (// Arrange, // Act, // Assert).

Aislamiento en Unit Testing: Reemplazar todas las interfaces/puertos con Mocks (Substitute.For<IInterface>()).

Pruebas de Arquitectura: Toda regla de diseño (dependencias entre capas, sufijos de clases, firmas de controladores) debe estar auditada y ejecutarse con dotnet test.

💾 Reglas de Datos e Infraestructura
Inversión de Control: Los repositorios se definen como interfaces en Tienda.Domain y se implementan en Tienda.Infrastructure usando EF Core.

Mapeo: Uso de Mappers explícitos (o métodos de extensión) para convertir Entidades de Dominio a Modelos de Persistencia (DB) y viceversa, impidiendo que EF Core contamine las entidades del dominio.
