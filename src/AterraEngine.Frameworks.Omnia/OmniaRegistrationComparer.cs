// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraEngine.Frameworks.Omnia;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class OmniaRegistrationComparer : IOmniaRegistrationComparer {
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool Equals(IOmniaRegistrationKey? x, IOmniaRegistrationKey? y) 
        => x is not null && y is not null && x.Equals(y) ;
    
    public int GetHashCode(IOmniaRegistrationKey obj)
        => obj.GetHashCode();

    public bool Equals(OmniaId alternate, IOmniaRegistrationKey other)
        => alternate == other.OmniaId;

    public int GetHashCode(OmniaId alternate) 
        => alternate.GetHashCode();
    
    public bool Equals(Type alternate, IOmniaRegistrationKey other) 
        => alternate == other.AssetType;
    
    public int GetHashCode(Type alternate) 
        => alternate.GetHashCode();
    
    // We should NEVER create registrations based on the partial keys
    public IOmniaRegistrationKey Create(OmniaId alternate) => throw new NotSupportedException("Cannot create registration based on partial key");
    public IOmniaRegistrationKey Create(Type alternate) => throw new NotSupportedException("Cannot create registration based on partial key");
}
