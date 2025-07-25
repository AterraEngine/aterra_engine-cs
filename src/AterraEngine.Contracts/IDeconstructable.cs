// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Contracts;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IDeconstructable<T> {
    void Deconstruct(out T a);
}

public interface IDeconstructable<T1, T2> {
    void Deconstruct(out T1 a, out T2 b);
}

public interface IDeconstructable<T1, T2, T3> {
    void Deconstruct(out T1 a, out T2 b, out T3 c);
}

public interface IDeconstructable<T1, T2, T3, T4> {
    void Deconstruct(out T1 a, out T2 b, out T3 c, out T4 d);
}