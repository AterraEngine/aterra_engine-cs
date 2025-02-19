// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Frameworks.Continuum.Pipelines;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPipelineStep<TInput, TOutput>  {
    bool IsNextStepPopulated { get; }
    
    [MemberNotNullWhen(true, nameof(IsNextStepPopulated))]
    Func<TInput, CancellationToken, TOutput>? NextStep { set; }
    
    TOutput HandleStepAsync(TInput input, CancellationToken ct = default);
}
