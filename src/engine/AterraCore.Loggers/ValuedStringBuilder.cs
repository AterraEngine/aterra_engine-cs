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
    private void Valued(string text, bool destruct, object?[] objects) {
        string at = destruct ? "@": "";
        
        _stringBuilder.Append(text);
        _stringBuilder.AppendJoin(" ", objects.Select((_, i) => $"{{{at}args{_propertyValues.Count + i}}}"));
        
        _propertyValues.AddRange(objects);
    }
    
    public ValuedStringBuilder AppendLineValued(string text, bool destruct, params object?[] objects) {
        _stringBuilder.AppendLine();
        Valued(text,destruct, objects);
        return this;
    }
    
    public ValuedStringBuilder AppendLineValued(string text, params object?[] objects) {
        return AppendLineValued(text, false, objects);
    }
    
    public ValuedStringBuilder AppendValued(string text, bool destruct, params object?[] objects) {
        Valued(text,destruct, objects);
        return this;
    }
    
    public ValuedStringBuilder AppendValued(string text, params object?[] objects) {
        return AppendValued(text, false, objects);
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