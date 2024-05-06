// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CliArgsParser.Attributes;
using JetBrains.Annotations;
using Serilog;

namespace ProductionTools.Commands;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[CommandAtlas]
public class TestConsoleTheme(ILogger logger) {
    // -----------------------------------------------------------------------------------------------------------------
    // Commands
    // -----------------------------------------------------------------------------------------------------------------
    [Command("console-theme-test")]
    [UsedImplicitly]
    public void GenerateXmlSchemaEngineConfig() {
        // Log messages with different levels and variable types
        logger.Verbose("This is a verbose message");
        logger.Debug("This is a debug message");
        logger.Information("This is an information message");
        logger.Warning("This is a warning message");
        logger.Error("This is an error message");
        logger.Fatal("This is a fatal message");

        Console.WriteLine();
        
        // Log messages with various variable types
        logger.Information("Integer value: {IntegerValue}", 42);
        logger.Information("Float value: {FloatValue}", 3.14);
        logger.Information("String value: {StringValue}", "Hello, world!");
        logger.Information("Boolean value: {BooleanValue}", true);
        logger.Information("Object value: {@ObjectValue}", new { Name = "John", Age = 30 });
        logger.Information("null value: {@ObjectValue}", [null]);
        logger.Information("list value: {ObjectValue}", new List<string>{ "a", "b", "c"});
        
        logger.Information("Type name value: {Object}", typeof(TestConsoleTheme));
        
        logger.Information("Incorrect value: {ObjectValue}", null);
        
        Console.WriteLine();
        
        // Log messages with structured properties
        logger.Information("Structured log message: {Property1} & {Property2}", "Value1", "Value2");
        
        Console.WriteLine();
        
        // Log an exception
        try {
            throw new InvalidOperationException("An example exception occurred");
        }
        catch (Exception ex) {
            logger.Error(ex, "An exception occurred");
        }
        
    }
}