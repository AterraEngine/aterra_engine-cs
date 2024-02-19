// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text;
using AterraEngine.Contracts;
using AterraEngine.Core.ServicesFramework;
using Serilog;

namespace AterraEngine.Core;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AterraEngine : IAterraEngine {
    protected ILogger Logger = DefaultServices.GetLogger();
    
    
    public void Run() {
        var stringbuilder = new StringBuilder();

        for (int i = 0; i < 10; i++) {
            stringbuilder.AppendLine();
        }
        
        Console.WriteLine(stringbuilder.ToString());
        
        throw new NotImplementedException();
    }
}