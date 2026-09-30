using NetArchTest.Rules;
using Xunit;

namespace Tienda.Infrastructure.Tests
{
    public class ArchitectureTests
    {
        private const string DomainNamespace = "Tienda.Domain";
        private const string ApplicationNamespace = "Tienda.Application";
        private const string InfrastructureNamespace = "Tienda.Infrastructure";

        [Fact]
        public void Domain_Should_Not_HaveDependencyOn_OtherProjects()
        {
            // 1. Obtener el ensamblado a inspeccionar mediante AssemblyReference
            var assembly = typeof(Tienda.Domain.AssemblyReference).Assembly;

            // 2. Definir la regla con NetArchTest
            var result = Types.InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(ApplicationNamespace, InfrastructureNamespace)
                .GetResult();

            // 3. Afirmar el resultado
            Assert.True(result.IsSuccessful, "La capa de Dominio no debe depender de Aplicación ni de Infraestructura.");
        }
    }
}