// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.AterraEngine.Frameworks.Nexities.Generators;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ValidKeyedInstanceAnalyzerTests {
    [Test]
    public async Task Analyzer_Diagnostic_ShouldNotOccur() {
        // Arrange
        var testCode = """
            using AterraEngine.Frameworks.Nexities;
            
            public class TestClass {
                [KeyedInstance("04fe2d73-8a3e-4fdd-8d1e-f6f5432c2470")] 
                public string TestProperty { get; set; } = string.Empty;
            }
            """;

        // Act
        var compilation = Compile(testCode);
        compilation.AddGenerator(MyCustomGenerator);

        // Assert
        await Assert.That(compilation).HasDiagnostic("AE0001");
    }
}
