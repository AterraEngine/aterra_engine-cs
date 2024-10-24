// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Common.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace AterraEngine.Generators.Sample;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ISampleService;

[InjectableService<ISampleService>(ServiceLifetime.Singleton)]
public class SampleService : ISampleService;
