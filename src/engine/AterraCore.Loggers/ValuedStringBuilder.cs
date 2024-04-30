// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Text;

namespace AterraCore.Loggers;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public class ValuedStringBuilder {
    private readonly StringBuilder _stringBuilder = new();
    private readonly List<object?> _propertyValues = new();


    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private void Valued(string text, object?[] objects) {
        _stringBuilder.Append(text);
        _stringBuilder.AppendJoin(" ", objects.Select((_, i) => $"{{args{_propertyValues.Count + i}}}"));
        
        _propertyValues.AddRange(objects);
    }
    
    public ValuedStringBuilder AppendLineValued(string text, params object?[] objects) {
        _stringBuilder.AppendLine();
        Valued(text, objects);
        return this;
    }
    
    public ValuedStringBuilder AppendValued(string text, params object?[] objects) {
        Valued(text, objects);
        return this;
    }


    public object?[] ValuesToArray() {
        return _propertyValues.ToArray();
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // String builder quick and dirty methods
    // -----------------------------------------------------------------------------------------------------------------
    public ValuedStringBuilder Append(object? value) {
        _stringBuilder.Append(value);
        return this;
    }
    
    public ValuedStringBuilder AppendLine() {
        _stringBuilder.AppendLine(Environment.NewLine);
        return this;
    }

    public ValuedStringBuilder AppendLine(string? value) {
        _stringBuilder.AppendLine(value);
        return this;
    }

    public override string ToString() {
        return _stringBuilder.ToString();
    }
}