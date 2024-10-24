// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using AterraEngine.Common.Attributes;
using AterraEngine.Generators.InjectableServices;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;
using System.Diagnostics;
using System.Linq;

namespace AterraEngine.Generators.Tests;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ServiceGeneratorTests {
    private const string VectorClassText = """
        using AterraEngine.Common.Attributes;
        using Microsoft.Extensions.DependencyInjection;
        namespace TestNamespace;

        public interface IVectorService;

        [InjectableService<IVectorService>(ServiceLifetime.Singleton)]
        public partial class VectorService : IVectorService { }

        public interface IArrayService;

        [InjectableService<IArrayService>(ServiceLifetime.Transient)]
        public partial class ArrayService : IArrayService { }

        public interface ISpanService;

        [InjectableService<ISpanService>(ServiceLifetime.Scoped)]
        public partial class SpanService : ISpanService { }

        """;

    private const string ExpectedGeneratedClassText = """
        using Microsoft.Extensions.DependencyInjection;
        namespace ServiceGeneratorTests;

        public static class InjectableServicesExtensions {
            public static IServiceCollection RegisterServicesFromServiceGeneratorTests(this IServiceCollection services) {
                services.AddScoped<TestNamespace.ISpanService, TestNamespace.SpanService>();
                services.AddSingleton<TestNamespace.IVectorService, TestNamespace.VectorService>();
                services.AddTransient<TestNamespace.IArrayService, TestNamespace.ArrayService>();
                return services;
            }
        }
        
        """;

    [Fact]
    public void GenerateReportMethod() {
        // Create an instance of the source generator.
        var generator = new InjectableServicesGenerator();

        // Source generators should be tested using 'GeneratorDriver'.
        var driver = CSharpGeneratorDriver.Create(generator.AsSourceGenerator());// This converts the IIncrementalGenerator to ISourceGenerator

        // We need to create a compilation with the required source code.
        var compilation = CSharpCompilation.Create(nameof(ServiceGeneratorTests),
            [CSharpSyntaxTree.ParseText(VectorClassText)],
            [
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(ServiceLifetime).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(InjectableServiceAttribute<>).Assembly.Location ?? throw new InvalidOperationException("Unable to locate InjectableServiceAttribute assembly"))
            ],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        // Run generators and retrieve all results.
        GeneratorDriverRunResult driverRunResult = driver.RunGenerators(compilation).GetRunResult();

        // Ensure something was generated
        Assert.NotEmpty(driverRunResult.Results);

        // For debugging purposes, list all generated hint names and diagnostics
        foreach (GeneratedSourceResult generatedSource in driverRunResult.Results.SelectMany(result => result.GeneratedSources)) {
            Debug.WriteLine($"Generated Source HintName: {generatedSource.HintName}");
        }

        foreach (Diagnostic diagnostic in driverRunResult.Diagnostics) {
            Debug.WriteLine($"Diagnostic: {diagnostic.GetMessage()}");
        }

        // Ensure the expected file is generated
        List<GeneratedSourceResult> generatedSources = driverRunResult.Results.SelectMany(r => r.GeneratedSources).ToList();
        Assert.NotEmpty(generatedSources);

        SourceText? generatedFileSyntax = generatedSources
            .SingleOrDefault(t => t.HintName.EndsWith(InjectableServicesGenerator.GeneratedFileName))
            .SourceText;

        Assert.NotNull(generatedFileSyntax);
        Assert.Equal(ExpectedGeneratedClassText.Trim(), 
            generatedFileSyntax.ToString().Trim(),
            ignoreLineEndingDifferences: true, 
            ignoreWhiteSpaceDifferences: true
        );
    }
}
