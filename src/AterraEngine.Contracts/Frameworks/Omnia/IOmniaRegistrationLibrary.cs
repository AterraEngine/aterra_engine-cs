// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Frameworks.Omnia;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IOmniaRegistrationLibrary {
    bool TryGetRegistration(OmniaId omniaId,[NotNullWhen(true)] out IOmniaRegistration? registration);
    bool TryGetRegistration(Type assetType,[NotNullWhen(true)] out IOmniaRegistration? registration);
}
