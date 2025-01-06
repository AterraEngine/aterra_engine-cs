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

    public readonly record struct Error<T>(T Value) : IValue<T> {
        public static implicit operator Error<T>(T value) => new(value);
    }
}
