// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace AterraCore.Nexities.Attributes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class CustomTagsAttribute : Attribute {
    public string[] CustomTags { get; private set; } 
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public CustomTagsAttribute(params string[] customTags) {
        CustomTags = customTags.Select(tag => tag.ToLowerInvariant()).ToArray();
    }

    public CustomTagsAttribute(params Enum[] customTags) {
        CustomTags = customTags.Select(tag => tag.ToString().ToLowerInvariant()).ToArray();
    }
}