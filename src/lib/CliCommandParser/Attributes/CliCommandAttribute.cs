// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliCommandParser.Contracts;

namespace CliCommandParser.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Method)]
public class CliCommandAttribute<T>(string name, string? description = null) : Attribute, ICliCommandAttribute where T: IParameterOptions , new() {
    public string Name { get; } = name;
    public string? Description { get; } = description;
    private readonly ParameterParser<T> _parameterParser = new();
    public IParameterOptions GetParameters(string[] args) => _parameterParser.Parse(args);
}