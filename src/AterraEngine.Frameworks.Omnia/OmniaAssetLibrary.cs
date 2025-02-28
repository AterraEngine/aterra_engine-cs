// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AterraEngine.Frameworks.Omnia;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class OmniaAssetLibrary(IOmniaRegistrationLibrary registrationLibrary, IScopedProvider provider) : IOmniaAssetLibrary {
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryGetInstance<T>(OmniaId omniaId, [NotNullWhen(true)] out T? asset) where T : class, IOmniaAsset {
        asset = null;
        if (!registrationLibrary.TryGetRegistration(omniaId, out IOmniaRegistration? registration)) return false;
        
        // Do checks if the registrations calls for a new one every time, per level, etc...
        //      Then why don't we do this through he coped provider?
        if (!registration.AssetType.IsAssignableFrom(typeof(T))) return false;
        if (!registration.TryCreateInstance(provider, out T? castedAsset)) return false;
        
        asset = castedAsset;
        return true;
    }
}
