// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Common.Attributes;
using AterraEngine.Contracts.Core.Omnia.Instances;
using AterraEngine.Contracts.Core.Omnia.Registrations;
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Core.Omnia.Instances;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IAssetInstanceAtlas>(ServiceLifetime.Singleton)]
public class AssetInstanceAtlas(IAssetRegistrationAtlas registrationAtlas) : IAssetInstanceAtlas {

}
