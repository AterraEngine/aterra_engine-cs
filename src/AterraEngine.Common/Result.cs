// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;

namespace AterraEngine;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UnionAliases(aliasT1: "Error")]
[UnionExtra(UnionExtra.GenerateFrom)]
public readonly partial struct Result<T>() : IUnion<T, Result<T>.Error<string>> {

    public readonly record struct Error<TError>(TError Value) : IValue<TError> {
        public static implicit operator Error<TError>(TError value) => new(value);
    }
}
