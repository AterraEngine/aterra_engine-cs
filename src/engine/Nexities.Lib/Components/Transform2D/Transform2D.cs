// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Numerics;
using AterraCore.Nexities.Assets.InstanceDto.Elements;
using AterraCore.Nexities.Components;
using Xml.Elements;

namespace Nexities.Lib.Components.Transform2D;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[Component<ITransform2D>("AC000000")]
public class Transform2D : NexitiesComponent, ITransform2D {
    
    [FromInstanceDto("Translation", value => ValueTask.parse(value))]
    public Vector2 Translation { get; set; } = Vector2.Zero;
    
    [FromNamedValue("Scale",)]
    public Vector2 Scale { get; set; } = Vector2.One;
    
    [NamedValue("Scale", )]
    public Vector2 Rotation { get; set; } = Vector2.Zero;

    public void FromInstanceDto(ComponentXmlDto componentXmlDto) {
        foreach (NamedValueDto valueDto in componentXmlDto.NamedValueDtos) {
            float[] data = valueDto.Value
                .Split(";")
                .Select(float.Parse)
                .ToArray();
            var vector =  new Vector2(data[0], data[1]);
            
            switch (valueDto.Name) {
                case "Translation": 
                    Translation =vector;
                    break;
                case "Scale": 
                    Scale = vector;
                    break;
                case "Rotation": 
                    Rotation = vector;
                    break;
            }
        }
    }
}