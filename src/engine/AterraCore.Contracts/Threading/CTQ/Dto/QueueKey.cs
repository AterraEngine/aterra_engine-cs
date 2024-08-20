// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Contracts.Threading.CTQ.Dto;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public enum QueueThreads{
    Main,
    Logic,
    Render,
}

public record QueueKey(
    QueueThreads Sender,
    QueueThreads Receiver
) {
    public static QueueKey MainToLogic = new (QueueThreads.Main, QueueThreads.Logic); 
    public static QueueKey MainToRender = new (QueueThreads.Main, QueueThreads.Render); 
    
    public static QueueKey LogicToMain = new (QueueThreads.Logic, QueueThreads.Main); 
    public static QueueKey LogicToRender = new (QueueThreads.Logic, QueueThreads.Render); 
    
    public static QueueKey RenderToMain = new (QueueThreads.Render, QueueThreads.Main); 
    public static QueueKey RenderToLogic = new (QueueThreads.Render, QueueThreads.Logic); 
    
}
