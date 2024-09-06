// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Contracts.Threading.CrossThread.Dto;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public enum QueueThreads {
    Main,
    Logic,
    Render
}

public record QueueKey(
    QueueThreads Sender,
    QueueThreads Receiver
) {
    public static readonly QueueKey MainToLogic = new(QueueThreads.Main, QueueThreads.Logic);
    public static readonly QueueKey MainToRender = new(QueueThreads.Main, QueueThreads.Render);

    public static readonly QueueKey LogicToMain = new(QueueThreads.Logic, QueueThreads.Main);
    public static readonly QueueKey LogicToRender = new(QueueThreads.Logic, QueueThreads.Render);

    public static readonly QueueKey RenderToMain = new(QueueThreads.Render, QueueThreads.Main);
    public static readonly QueueKey RenderToLogic = new(QueueThreads.Render, QueueThreads.Logic);
}
