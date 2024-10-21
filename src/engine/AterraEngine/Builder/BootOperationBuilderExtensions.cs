// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Contracts.Builder;

namespace AterraEngine.Builder;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class BootOperationBuilderExtensions {
    public static bool TryRegister<T1>(this IBootOperationBuilder builder, string chainName) => builder.TryRegister(chainName, typeof(T1));
    public static bool TryRegister<T1, T2>(this IBootOperationBuilder builder, string chainName) => builder.TryRegister(chainName, typeof(T1), typeof(T2));
    public static bool TryRegister<T1, T2, T3>(this IBootOperationBuilder builder, string chainName) => builder.TryRegister(chainName, typeof(T1), typeof(T2), typeof(T3));
    public static bool TryRegister<T1, T2, T3, T4>(this IBootOperationBuilder builder, string chainName) => builder.TryRegister(chainName, typeof(T1), typeof(T2), typeof(T3), typeof(T4));
    public static bool TryRegister<T1, T2, T3, T4, T5>(this IBootOperationBuilder builder, string chainName) => builder.TryRegister(chainName, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
    public static bool TryRegister<T1, T2, T3, T4, T5, T6>(this IBootOperationBuilder builder, string chainName) => builder.TryRegister(chainName, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
    public static bool TryRegister<T1, T2, T3, T4, T5, T6, T7>(this IBootOperationBuilder builder, string chainName) => builder.TryRegister(chainName, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
    public static bool TryRegister<T1, T2, T3, T4, T5, T6, T7, T8>(this IBootOperationBuilder builder, string chainName) => builder.TryRegister(chainName, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));

    public static bool TryAddToChain<T1>(this IBootOperationBuilder builder, string chainName) => builder.TryAddToChain(chainName, typeof(T1));
    public static bool TryAddToChain<T1, T2>(this IBootOperationBuilder builder, string chainName) => builder.TryAddToChain(chainName, typeof(T1), typeof(T2));
    public static bool TryAddToChain<T1, T2, T3>(this IBootOperationBuilder builder, string chainName) => builder.TryAddToChain(chainName, typeof(T1), typeof(T2), typeof(T3));
    public static bool TryAddToChain<T1, T2, T3, T4>(this IBootOperationBuilder builder, string chainName) => builder.TryAddToChain(chainName, typeof(T1), typeof(T2), typeof(T3), typeof(T4));
    public static bool TryAddToChain<T1, T2, T3, T4, T5>(this IBootOperationBuilder builder, string chainName) => builder.TryAddToChain(chainName, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
    public static bool TryAddToChain<T1, T2, T3, T4, T5, T6>(this IBootOperationBuilder builder, string chainName) => builder.TryAddToChain(chainName, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
    public static bool TryAddToChain<T1, T2, T3, T4, T5, T6, T7>(this IBootOperationBuilder builder, string chainName) => builder.TryAddToChain(chainName, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
    public static bool TryAddToChain<T1, T2, T3, T4, T5, T6, T7, T8>(this IBootOperationBuilder builder, string chainName) => builder.TryAddToChain(chainName, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));

    public static IBootOperationBuilder Register<T1>(this IBootOperationBuilder builder, string chainName) => builder.Register(chainName, typeof(T1));
    public static IBootOperationBuilder Register<T1, T2>(this IBootOperationBuilder builder, string chainName) => builder.Register(chainName, typeof(T1), typeof(T2));
    public static IBootOperationBuilder Register<T1, T2, T3>(this IBootOperationBuilder builder, string chainName) => builder.Register(chainName, typeof(T1), typeof(T2), typeof(T3));
    public static IBootOperationBuilder Register<T1, T2, T3, T4>(this IBootOperationBuilder builder, string chainName) => builder.Register(chainName, typeof(T1), typeof(T2), typeof(T3), typeof(T4));
    public static IBootOperationBuilder Register<T1, T2, T3, T4, T5>(this IBootOperationBuilder builder, string chainName) => builder.Register(chainName, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
    public static IBootOperationBuilder Register<T1, T2, T3, T4, T5, T6>(this IBootOperationBuilder builder, string chainName) => builder.Register(chainName, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
    public static IBootOperationBuilder Register<T1, T2, T3, T4, T5, T6, T7>(this IBootOperationBuilder builder, string chainName) => builder.Register(chainName, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
    public static IBootOperationBuilder Register<T1, T2, T3, T4, T5, T6, T7, T8>(this IBootOperationBuilder builder, string chainName) => builder.Register(chainName, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));

    public static IBootOperationBuilder AddToChain<T1>(this IBootOperationBuilder builder, string chainName) => builder.AddToChain(chainName, typeof(T1));
    public static IBootOperationBuilder AddToChain<T1, T2>(this IBootOperationBuilder builder, string chainName) => builder.AddToChain(chainName, typeof(T1), typeof(T2));
    public static IBootOperationBuilder AddToChain<T1, T2, T3>(this IBootOperationBuilder builder, string chainName) => builder.AddToChain(chainName, typeof(T1), typeof(T2), typeof(T3));
    public static IBootOperationBuilder AddToChain<T1, T2, T3, T4>(this IBootOperationBuilder builder, string chainName) => builder.AddToChain(chainName, typeof(T1), typeof(T2), typeof(T3), typeof(T4));
    public static IBootOperationBuilder AddToChain<T1, T2, T3, T4, T5>(this IBootOperationBuilder builder, string chainName) => builder.AddToChain(chainName, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
    public static IBootOperationBuilder AddToChain<T1, T2, T3, T4, T5, T6>(this IBootOperationBuilder builder, string chainName) => builder.AddToChain(chainName, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
    public static IBootOperationBuilder AddToChain<T1, T2, T3, T4, T5, T6, T7>(this IBootOperationBuilder builder, string chainName) => builder.AddToChain(chainName, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
    public static IBootOperationBuilder AddToChain<T1, T2, T3, T4, T5, T6, T7, T8>(this IBootOperationBuilder builder, string chainName) => builder.AddToChain(chainName, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));


}
