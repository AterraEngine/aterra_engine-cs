// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using ArgsParser.Interfaces;

namespace ArgsParser.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Method)]
public class CliCommandAttribute<T>(string commandName) : Attribute, ICliCommandAttribute where T: IParameterOptions , new() {
    public string CommandName { get; } = commandName;
    private readonly ParameterParser<T> _propertyParser = new();
    
    public IParameterOptions GetParameters(string[] args) => _propertyParser.Parse(args);
}