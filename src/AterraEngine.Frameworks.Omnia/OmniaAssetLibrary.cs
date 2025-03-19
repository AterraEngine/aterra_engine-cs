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
        if (!typeof(T).IsAssignableFrom(registration.AssetType)) return false;
        if (!registration.TryCreateInstance(provider, out T? castedAsset)) return false;
        
        castedAsset.Initialize(omniaId);
        asset = castedAsset;
        
        return true;
    }
    
    public bool TryGetInstance<T>([NotNullWhen(true)] out T? asset) where T : class, IOmniaAsset {
        asset = null;
        if (!registrationLibrary.TryGetRegistration(typeof(T), out IOmniaRegistration? registration)) return false;
        if (!typeof(T).IsAssignableFrom(registration.AssetType)) return false;
        if (!registration.TryCreateInstance(provider, out T? castedAsset)) return false;
        
        castedAsset.Initialize(registration.OmniaId);
        asset = castedAsset;
        
        return true;
    }

    public void ReturnInstance<T>(T asset) where T : class, IOmniaAsset {
        registrationLibrary.TryGetRegistration(asset.OmniaId, out IOmniaRegistration? registration);
        registration?.ReturnToPool(asset);
    }
}
