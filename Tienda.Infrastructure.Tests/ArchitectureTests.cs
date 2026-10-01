using NetArchTest.Rules;
using Xunit;

namespace Tienda.Infrastructure.Tests;

public class ArchitectureTests
{
    private const string DomainNamespace = "Tienda.Domain";
    private const string ApplicationNamespace = "Tienda.Application";
    private const string InfrastructureNamespace = "Tienda.Infrastructure";

    [Fact]
    public void Domain_ShouldNotHaveDependencyOn_OtherProjects()
    {
        // Arrange: Inspeccionar el ensamblado de Dominio mediante AssemblyReference
        var assembly = Domain.AssemblyReference.Assembly;
        var forbiddenProjects = new[] { ApplicationNamespace, InfrastructureNamespace };

        // Act
        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(forbiddenProjects)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "La capa de Dominio no debe depender de Aplicación ni de Infraestructura.");
    }

    [Fact]
    public void Application_ShouldNotHaveDependencyOn_Infrastructure()
    {
        // Arrange
        var assembly = Application.AssemblyReference.Assembly;

        // Act
        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "La capa de Aplicación no debe depender de Infraestructura.");
    }

    [Fact]
    public void Handlers_ShouldHaveNameEndingWith_Handler()
    {
        // Arrange
        var assembly = Application.AssemblyReference.Assembly;

        // Act
        var result = Types.InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(MediatR.IRequestHandler<,>))
            .Should()
            .HaveNameEndingWith("Handler")
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "Todos los Handlers en Aplicación deben terminar con el sufijo 'Handler'.");
    }
}