// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

namespace AterraCore.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class)]
[UsedImplicitly]
public class InjectableAttribute(ServiceLifetime serviceLifetime = ServiceLifetime.Transient, bool @static = false, params Type[] typeInterfaces) : Attribute {
    public readonly Type[] Interfaces = typeInterfaces;
    public readonly ServiceLifetime Lifetime = serviceLifetime;
    public readonly bool IsStatic = @static;
}

[UsedImplicitly, ExcludeFromCodeCoverage] public class InjectableAttribute<T>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient, bool @static = false) : InjectableAttribute(serviceLifetime, @static, typeof(T));
[UsedImplicitly, ExcludeFromCodeCoverage] public class InjectableAttribute<T1, T2>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient, bool @static = false) : InjectableAttribute(serviceLifetime, @static, typeof(T1), typeof(T2));
[UsedImplicitly, ExcludeFromCodeCoverage] public class InjectableAttribute<T1, T2, T3>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient, bool @static = false) : InjectableAttribute(serviceLifetime, @static, typeof(T1), typeof(T2), typeof(T3));
[UsedImplicitly, ExcludeFromCodeCoverage] public class InjectableAttribute<T1, T2, T3, T4>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient, bool @static = false) : InjectableAttribute(serviceLifetime, @static, typeof(T1), typeof(T2), typeof(T3), typeof(T4));
[UsedImplicitly, ExcludeFromCodeCoverage] public class InjectableAttribute<T1, T2, T3, T4, T5>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient, bool @static = false) : InjectableAttribute(serviceLifetime, @static, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
[UsedImplicitly, ExcludeFromCodeCoverage] public class InjectableAttribute<T1, T2, T3, T4, T5, T6>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient, bool @static = false) : InjectableAttribute(serviceLifetime, @static, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
[UsedImplicitly, ExcludeFromCodeCoverage] public class InjectableAttribute<T1, T2, T3, T4, T5, T6, T7>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient, bool @static = false) : InjectableAttribute(serviceLifetime, @static, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
[UsedImplicitly, ExcludeFromCodeCoverage] public class InjectableAttribute<T1, T2, T3, T4, T5, T6, T7, T8>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient, bool @static = false) : InjectableAttribute(serviceLifetime, @static, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));
[UsedImplicitly, ExcludeFromCodeCoverage] public class InjectableAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient, bool @static = false) : InjectableAttribute(serviceLifetime, @static, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9));
[UsedImplicitly, ExcludeFromCodeCoverage] public class InjectableAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient, bool @static = false) : InjectableAttribute(serviceLifetime, @static, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10));
[UsedImplicitly, ExcludeFromCodeCoverage] public class InjectableAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient, bool @static = false) : InjectableAttribute(serviceLifetime, @static, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11));
[UsedImplicitly, ExcludeFromCodeCoverage] public class InjectableAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient, bool @static = false) : InjectableAttribute(serviceLifetime, @static, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12));
[UsedImplicitly, ExcludeFromCodeCoverage] public class InjectableAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient, bool @static = false) : InjectableAttribute(serviceLifetime, @static, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13));
[UsedImplicitly, ExcludeFromCodeCoverage] public class InjectableAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient, bool @static = false) : InjectableAttribute(serviceLifetime, @static, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14));
[UsedImplicitly, ExcludeFromCodeCoverage] public class InjectableAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient, bool @static = false) : InjectableAttribute(serviceLifetime, @static, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15));
[UsedImplicitly, ExcludeFromCodeCoverage] public class InjectableAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(ServiceLifetime serviceLifetime = ServiceLifetime.Transient, bool @static = false) : InjectableAttribute(serviceLifetime, @static, typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(T16));
