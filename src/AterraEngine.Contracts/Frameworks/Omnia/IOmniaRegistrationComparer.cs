// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Omnia;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IOmniaRegistrationComparer :
    IEqualityComparer<IOmniaRegistrationKey>,
    IAlternateEqualityComparer<OmniaId, IOmniaRegistrationKey>,
    IAlternateEqualityComparer<Type, IOmniaRegistrationKey>;
