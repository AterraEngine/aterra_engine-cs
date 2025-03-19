// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using AterraEngine.Frameworks.Omnia.PreProcessor;
using System.Collections.Frozen;

namespace AterraEngine.Frameworks.Omnia;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class OmniaRegistrationLibraryFactory(IOmniaRegistrationCollector registrationCollector, IOmniaRegistrationComparer comparer) : IOmniaRegistrationLibraryFactory {
    private readonly Lazy<FrozenDictionary<IOmniaRegistrationKey, IOmniaRegistration>> _emptyRegistration = new(
        () => new Dictionary<IOmniaRegistrationKey, IOmniaRegistration>().ToFrozenDictionary(comparer:comparer)
    );

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static OmniaRegistrationLibrary CreateFromDictionary(FrozenDictionary<IOmniaRegistrationKey, IOmniaRegistration> frozenRegistrations ) {
        return new OmniaRegistrationLibrary {
            Registrations = frozenRegistrations,
            RegistrationsByOmniaId = frozenRegistrations.GetAlternateLookup<OmniaId>(),
            RegistrationsByType = frozenRegistrations.GetAlternateLookup<Type>()
        };
    }
    
    public IOmniaRegistrationLibrary Create(IScopedProvider _) {
        if (registrationCollector.IsEmpty) return CreateFromDictionary(_emptyRegistration.Value);
        
        FrozenDictionary<IOmniaRegistrationKey, IOmniaRegistration> frozenRegistrations = registrationCollector
            .GetRegistrations()
            .ToFrozenDictionary(
                registration => registration.GetLookupKey() ,
                registration => registration,
                comparer: comparer
            );
        
        return CreateFromDictionary(frozenRegistrations);
    }
}
