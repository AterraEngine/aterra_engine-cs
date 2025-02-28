// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Frameworks.Omnia;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class OmniaRegistrationLibrary : IOmniaRegistrationLibrary {
    public FrozenDictionary<OmniaId, IOmniaRegistration> Registrations { private get; init; } = FrozenDictionary<OmniaId, IOmniaRegistration>.Empty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetRegistration(OmniaId omniaId,[NotNullWhen(true)] out IOmniaRegistration? registration) {
        registration = null;
        if (omniaId.IsEmpty) return false;
        
        return Registrations.TryGetValue(omniaId, out registration);
    }
}
