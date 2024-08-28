// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.Boot.Logic.PluginLoading.Dto;
using CliArgsParser;
using AterraCore.Contracts.Boot.Operations;
using AterraCore.Loggers;
using System.Reflection;

namespace AterraCore.Boot.Operations;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CliArgsParserAssembler : IBootOperation {
    private ILogger Logger { get; } = StartupLogger.CreateLogger(false).ForPluginLoaderContext("CliArgsParserAssembler");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void Run(IBootComponents components) {
        Logger.Debug("Entered CliArgsParser Assembler");
        
        components.Services.AddCliArgsParser(configuration => {
            configuration.SetConfig(new CliArgsParserConfig {
                Overridable = true,
                GenerateShortNames = true
            });

            foreach (IPluginBootDto pluginBootDto in components.ValidPlugins) {
                foreach (Assembly assembly in pluginBootDto.Assemblies.ToList()) {
                    configuration.AddFromAssembly(assembly);
                }
            }
        });
        
    }
}
