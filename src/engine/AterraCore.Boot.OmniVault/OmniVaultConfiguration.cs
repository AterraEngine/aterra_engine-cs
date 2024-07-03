// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Common.ConfigFiles.EngineConfig;
using AterraCore.Common.Data;
using AterraCore.Contracts.Boot.OmniVault;
using AterraCore.Contracts.OmniVault;
using AterraCore.OmniVault.Textures;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using static AterraCore.Common.Data.ConfigurationWarnings;
using static CodeOfChaos.Extensions.DependencyInjection.ServiceDescriptorExtension;

namespace AterraCore.Boot.OmniVault;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class OmniVaultConfiguration(ILogger logger, EngineConfigXml engineConfigDto) : IOmniVaultConfiguration {
    public LinkedList<ServiceDescriptor> ServicesDefault { get; } = new([
        NewServiceDescriptor<ITextureAtlas, TextureAtlas>(ServiceLifetime.Singleton),
    ]);
    
    public LinkedList<ServiceDescriptor> ServicesStatic { get; } = new([
    ]);

    public EngineConfigXml EngineConfig { get; set; } = engineConfigDto;

    public ConfigurationWarnings Warnings { get; private set; } = Nominal;
}
