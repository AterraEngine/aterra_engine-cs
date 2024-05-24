// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Resources;
using CliArgsParser.Attributes;
using Nexities.Lib.Components.AssetTree;
using Nexities.Lib.Components.Sprite2D;
using Nexities.Lib.Components.Transform2D;
using Nexities.Lib.Entities.Actor;
using Serilog;
namespace ProductionTools.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[CommandAtlas]
public class ResxTest(ILogger logger) {
    [Command("test-resx")]
    public void TestResxCallback() {
        var actor = new Actor2D(
            new Transform2D(),
            new Sprite2D(),
            new AssetTree()
        );

        var resx = new ResourceManager("TestResx.resx", typeof(TestResx).Assembly);
    }
}