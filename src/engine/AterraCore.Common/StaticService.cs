// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Common;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public struct StaticService {
    public Type Interface { get; private set; }
    public Type Implementation { get; private set; }
    public ServiceType Type { get; private set; }
    
    public static StaticService AsSingleton<T1, T2>() where T2 : T1 {
        return new StaticService {
            Interface = typeof(T1),
            Implementation = typeof(T2),
            Type = ServiceType.Singleton
        };
    }
}