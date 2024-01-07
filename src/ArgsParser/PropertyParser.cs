// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;

namespace ArgsParser;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PropertyParser<T> where T:new() {
    // Dictionaries needed to optimize access to the attributes.
    //      Go over and store them once, instead of on every argument like we did before.
    //      Maybe a bit overkill, but might be a good idea in the long run.
    private readonly Dictionary<string, PropertyInfo> _optionProperties = new();
    private readonly Dictionary<string, PropertyInfo> _flagProperties = new();
    
    public PropertyParser() {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in properties)
        {
            var optionAttr = prop.GetCustomAttribute<ArgAttribute>();
            var flagAttr = prop.GetCustomAttribute<ArgFlagAttribute>();

            if (optionAttr != null) {
                _optionProperties[$"-{optionAttr.ShortName}"] = prop;
                _optionProperties[$"--{optionAttr.LongName}"] = prop;
            }
            else if (flagAttr != null) {
                _flagProperties[$"-{flagAttr.ShortName}"] = prop;
                _flagProperties[$"--{flagAttr.LongName}"] = prop;
            }
        }
    }

    public T Parse(string[] args) {
        var result = new T();

        for (int i = 0; i < args.Length; i++) {
            if (_optionProperties.TryGetValue(args[i], out var optionProp) && i < args.Length - 1) {
                var value = Convert.ChangeType(args[++i], optionProp.PropertyType);
                optionProp.SetValue(result, value);
            }
            else if (_flagProperties.TryGetValue(args[i], out var flagProp)) {
                flagProp.SetValue(result, true);
            }
        }

        return result;
    }
}