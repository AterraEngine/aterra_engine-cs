// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Verifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<AterraEngine.Analyzers.EntityAttributeAnalyzer>;

namespace AterraEngine.Analyzer.Tests;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// WARN TOTALLY BROKEN
public class EntityAttributeAnalyzerTest {
    // private readonly DiagnosticAnalyzer _analyzer = new EntityAttributeAnalyzer();

    [Theory]
    [InlineData(@"public class MyEntity { }")]
    [InlineData(@"using AterraCore.Nexities.Entities; using AterraCore.Common; [EntityAttribute(AssetInstanceType.Singleton)] public class MyEntity { }")]
    public async Task Test_AnalyzeSymbol_NoTrigger(string code) {
        // Define the expected DiagnosticResult pointing to the actual location in the code
        // DiagnosticResult expected = Verifier.Diagnostic(EntityAttributeAnalyzer.DiagnosticId);

        // Assert the expected and actual results using the static VerifyAnalyzerAsync method
        await Verifier.VerifyAnalyzerAsync(code);
    }

    [Theory]
    [InlineData(@"using AterraCore.Nexities.Entities; using AterraCore.Common; [EntityAttribute(AssetInstanceType.Pooled)] public class MyEntity { }", 1, 30)]
    public async Task Test_AnalyzeSymbol_WithAttribute_InstanceTypePooled(string code, int line, int column) {
        DiagnosticResult expected = Verifier.Diagnostic(EntityAttributeAnalyzer.DiagnosticId)
            .WithSeverity(DiagnosticSeverity.Warning)
            .WithLocation(line, column); // Replace with the actual location

        await Verifier.VerifyAnalyzerAsync(code, expected);
    }
}