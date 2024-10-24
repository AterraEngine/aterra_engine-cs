// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Common.Attributes;
using AterraEngine.Contracts.Core.Omnia.Registrations;
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Core.Omnia.Registrations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IAssetRegistrationAtlas>(ServiceLifetime.Singleton)]
public class AssetRegistrationAtlas : IAssetRegistrationAtlas {

}
