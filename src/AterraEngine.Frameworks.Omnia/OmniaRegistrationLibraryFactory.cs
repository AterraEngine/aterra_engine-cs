// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using System.Collections.Concurrent;
using System.Collections.Frozen;

namespace AterraEngine.Frameworks.Omnia;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class OmniaRegistrationLibraryFactory : IOmniaRegistrationLibraryFactory {
    private ConcurrentQueue<IOmniaRegistration> Registrations { get; } = new();
    private FrozenDictionary<OmniaId, IOmniaRegistration> FrozenRegistrations { get; set; } = FrozenDictionary<OmniaId, IOmniaRegistration>.Empty;
    public bool IsFrozen { get; private set; }
    public bool IsEmpty => IsFrozen ? FrozenRegistrations.IsEmpty() : Registrations.IsEmpty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void AddRegistration(IOmniaRegistration registration) {
        if (IsFrozen) throw new InvalidOperationException("Cannot add registration to a frozen library.");
        Registrations.Enqueue(registration);
    }

    public IOmniaRegistrationLibraryFactory ToFrozen() {
        if (IsFrozen) return this;

        var tempDictionary = new Dictionary<OmniaId, IOmniaRegistration>();
        while (Registrations.TryDequeue(out IOmniaRegistration? result)) {
            // Still do some extra checks here to ensure valid registrations
            tempDictionary.Add(result.OmniaId, result);
        }

        FrozenRegistrations = tempDictionary.ToFrozenDictionary();

        IsFrozen = true;
        return this;
    }

    public IOmniaRegistrationLibrary Create(IScopedProvider provider) {
        if (IsEmpty) return new OmniaRegistrationLibrary();
        if (!IsFrozen) ToFrozen();
        
        return new OmniaRegistrationLibrary {
            Registrations = FrozenRegistrations
        };
    }
}
