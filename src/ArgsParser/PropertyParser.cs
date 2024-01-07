// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Reflection;

namespace ArgsParser;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// A generic class for parsing command-line arguments and populating a property object.
/// </summary>
/// <typeparam name="T">The type of the property object to be populated.</typeparam>
public class PropertyParser<T> where T:new() {
    // Dictionaries needed to optimize access to the attributes.
    //      Go over and store them once, instead of on every argument like we did before.
    //      Maybe a bit overkill, but might be a good idea in the long run.
    /// <summary>
    /// This variable is a private instance of a Dictionary that stores option property names as keys and their corresponding PropertyInfo objects as values.
    /// </summary>
    private readonly Dictionary<string, PropertyInfo> _optionProperties = new();

    /// <summary>
    /// Represents a dictionary mapping flag names to PropertyInfo objects.
    /// </summary>
    private readonly Dictionary<string, PropertyInfo> _flagProperties = new();

    /// <summary>
    /// Array of PropertyInfo objects that contain information about the public instance properties of the generic type T.
    /// </summary>
    private readonly PropertyInfo[] _propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

    /// <summary>
    /// Initializes a new instance of the PropertyParser class.
    /// </summary>
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

    /// <summary>
    /// Retrieves a collection of readable descriptions based on the values obtained from GetDescriptions method.
    /// </summary>
    /// <returns>
    /// Returns an IEnumerable collection of strings, each representing a readable description.
    /// Each description is formatted as follows:
    /// - [ShortName] -- [LongName] : [Description]
    /// </returns>
    public IEnumerable<string> GetDescriptionsReadable() {
        return GetDescriptions()
            .Select(v => $"-{v?.ShortName,-5} --{v?.LongName,-8} : {v?.Description ?? "UNKNOWN DESCRIPTION"}");
    }

    /// <summary>
    /// Retrieves descriptions for properties using the ArgsParserAttribute.
    /// </summary>
    /// <returns>
    /// An enumerable collection of ArgsParserAttribute instances representing the descriptions
    /// for properties. If a property does not have a ArgsParserAttribute, the corresponding
    /// element will be null.
    /// </returns>
    public IEnumerable<ArgsParserAttribute?> GetDescriptions() {
        return _propertyInfos
            .Select(value => value.GetCustomAttribute<ArgsParserAttribute>());
    }

    /// <summary>
    /// Parses the given array of string arguments and initializes an instance of the generic type T with the values provided.
    /// </summary>
    /// <typeparam name="T">The type of object to initialize.</typeparam>
    /// <param name="args">The array of string arguments to parse.</param>
    /// <returns>An instance of the generic type T initialized with the parsed values.</returns>
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