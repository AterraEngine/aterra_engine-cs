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
    private readonly PropertyInfo[] _propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    
    public PropertyParser() {
        foreach (var prop in _propertyInfos)
        {
            var optionAttr = prop.GetCustomAttribute<ArgValueAttribute>();
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
    
    // Easily retrieve the 
    public IEnumerable<string> GetReadableDescriptions() {
        return GetDescriptions()
            .Select(v => $"-{v?.ShortName.ToString(),-5} --{v?.LongName,-8} : {v?.Description}");
    }

    public IEnumerable<ArgsParserAttribute?> GetDescriptions() {
        return _propertyInfos
            .Select(value => value.GetCustomAttribute<ArgsParserAttribute>());
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