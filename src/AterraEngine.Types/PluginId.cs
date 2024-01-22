﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Types;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public struct PluginId : IComparable<PluginId> {
    public int Id { get; private set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public PluginId(int value) {
        Id = value;
    }
    public PluginId(string value) {
        ParseFromString(value);
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public override string ToString() {
        return Id.ToString("X");
    }

    private void ParseFromString(string value) {
        Id = int.Parse(value, System.Globalization.NumberStyles.HexNumber);
    }

    public int CompareTo(PluginId other) {
        return Id.CompareTo(other.Id);
    }
}