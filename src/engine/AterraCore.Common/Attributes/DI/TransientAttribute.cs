// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Common.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class)]
[UsedImplicitly]
public class TransientAttribute(params Type[] typeInterfaces)
    : InjectableAttribute(ServiceLifetime.Transient, typeInterfaces);

[UsedImplicitly] [ExcludeFromCodeCoverage] public class TransientAttribute<T>() : TransientAttribute(typeof(T));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class TransientAttribute<T1, T2>() : TransientAttribute(typeof(T1), typeof(T2));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class TransientAttribute<T1, T2, T3>() : TransientAttribute(typeof(T1), typeof(T2), typeof(T3));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class TransientAttribute<T1, T2, T3, T4>() : TransientAttribute(typeof(T1), typeof(T2), typeof(T3), typeof(T4));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class TransientAttribute<T1, T2, T3, T4, T5>() : TransientAttribute(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class TransientAttribute<T1, T2, T3, T4, T5, T6>() : TransientAttribute(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class TransientAttribute<T1, T2, T3, T4, T5, T6, T7>() : TransientAttribute(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class TransientAttribute<T1, T2, T3, T4, T5, T6, T7, T8>() : TransientAttribute(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class TransientAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9>() : TransientAttribute(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class TransientAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>() : TransientAttribute(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class TransientAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>() : TransientAttribute(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class TransientAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>() : TransientAttribute(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class TransientAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>() : TransientAttribute(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class TransientAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>() : TransientAttribute(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class TransientAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>() : TransientAttribute(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15));

[UsedImplicitly] [ExcludeFromCodeCoverage] public class TransientAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>() : TransientAttribute(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(T16));
