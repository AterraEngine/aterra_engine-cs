// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Concurrent;

namespace AterraEngine.Frameworks.Omnia.PreProcessor;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class OmniaRegistrationCollector : IOmniaRegistrationCollector {
    private ConcurrentDictionary<OmniaId, IOmniaRegistration> _registrations = new(); // < OmniaId, IOmniaRegistration>
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IOmniaRegistrationCollector AddRegistration(IOmniaRegistration registration,  CancellationToken ct = default) {
        // Validate the registration
        if (registration.OmniaId.IsEmpty) throw new ArgumentException("OmniaId cannot be empty", nameof(registration));
        
        // Attempt to look for it already
        //      Do some extra logic if we are registering a new registration tot the same id
        if (_registrations.TryGetValue(registration.OmniaId, out IOmniaRegistration? existing)) {
            if (existing == registration) return this;
            // Todo check if plugin of the new one is higher in load order than the one new,
            //      if so, overwrite.
            return this;
        }
        
        ct.ThrowIfCancellationRequested();

        // Check if we need to then do the above step again if this fails due to concurrent or parallel access
        _registrations.TryAdd(registration.OmniaId, registration);
        return this;
    }
    
    public IEnumerable<IOmniaRegistration> GetRegistrations() => _registrations.Values;
}

