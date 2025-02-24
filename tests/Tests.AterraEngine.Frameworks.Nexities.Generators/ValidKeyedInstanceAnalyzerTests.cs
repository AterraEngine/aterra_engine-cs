// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Frameworks.Nexities.Generators;
using AterraEngine.Frameworks.Nexities.Generators.Content.SyntaxCheckers;
using CodeOfChaos.Testing.TUnit;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Tests.AterraEngine.Frameworks.Nexities.Generators;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ValidKeyedInstanceAnalyzerTests {
    [Test]
    public async Task Analyzer_Diagnostic_ShouldNotOccur() {
        // Arrange
        string testCode = """
            using AterraEngine.Frameworks.Nexities;

            public class TestClass {
                [KeyedInstance("04fe2d73-8a3e-4fdd-8d1e-f6f5432c2470")] 
                public string TestProperty { get; set; } = string.Empty;
            }
            """;

        var runner = new RoslynCompilationRunner()
            .AddDocument("TestClass.cs", testCode)
            .AddDiagnosticAnalyzer<ValidKeyedInstanceAnalyzer>();

        // Act
        CompilationWithAnalyzers compilation = await runner.GetCompilationWithAnalyzersAsync();

        // Assert
        await Assert.That(compilation).DoesNotContainDiagnostic(Diagnostics.InvalidGuidDescriptor.Id);
    }

    [Test]
    public async Task Analyzer_Diagnostic_ShouldOccur() {
        // Arrange
        string testCode = """
            using AterraEngine.Frameworks.Nexities;

            public class TestClass {
                [KeyedInstance("XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX")] 
                public string TestProperty { get; set; } = string.Empty;
            }
            """;

        var runner = new RoslynCompilationRunner()
            .AddDocument("TestClass.cs", testCode)
            .AddDiagnosticAnalyzer<ValidKeyedInstanceAnalyzer>();

        // Act
        CompilationWithAnalyzers compilation = await runner.GetCompilationWithAnalyzersAsync();

        // Assert
        await Assert.That(compilation).ContainsDiagnostic(Diagnostics.InvalidGuidDescriptor.Id);
    }
}
