// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Omnia.PreProcessor;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IOmniaRegistrationCollector {
    IOmniaRegistrationCollector AddRegistration(IOmniaRegistration registration, CancellationToken ct = default) ;
    IEnumerable<IOmniaRegistration> GetRegistrations();
}
