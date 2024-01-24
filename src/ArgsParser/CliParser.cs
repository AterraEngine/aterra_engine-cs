// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.ObjectModel;
using System.Reflection;
using System.Xml;
using ArgsParser.Attributes;
using ArgsParser.Interfaces;

namespace ArgsParser;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class CliParser {
    private readonly Dictionary<string, Action<string[]>> _flagToActionMap = new();

    public void RegisterCli<T>(T cliCommandAtlas, bool force = false) where T:ICliCommandAtlas{
        var methods = typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Instance);

        foreach (var methodInfo in methods) {
            // Quick exits to find the correct methods
            if (methodInfo.GetCustomAttributes().FirstOrDefault(a => a is ICliCommandAttribute) is not ICliCommandAttribute cliCommandAttribute) continue;
            var methodParameters = methodInfo.GetParameters();
            if (methodParameters.Length == 0) continue;

            var parameterType = methodParameters[0].ParameterType;
            var openDelegateType = typeof(Action<>); // Why does this need to be an empty action???
            var delegateType = openDelegateType.MakeGenericType(parameterType); // DON'T use IParameterOptions, it doesn't work!
        
            Delegate del = Delegate.CreateDelegate(delegateType, cliCommandAtlas, methodInfo);
            
            var success = _flagToActionMap.TryAdd(
                cliCommandAttribute.CommandName, 
                args => {
                    del.DynamicInvoke(cliCommandAttribute.GetParameters(args));
                }
            );
            if (!success) {
                throw new Exception($"Command '{cliCommandAttribute.CommandName}' could not be bound to method: '{typeof(T)}.{methodInfo.Name}'");
            }
        }
    }

    public bool TryParse(string[] args) {
        if (!_flagToActionMap.TryGetValue(args[0], out var action)) return false;
        
        // Strip out the command and keep the arguments
        action(args[1..]);

        return true;
    }
    
}