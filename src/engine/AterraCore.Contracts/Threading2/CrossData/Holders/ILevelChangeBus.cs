// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace AterraCore.Contracts.Threading2.CrossData.Holders;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILevelChangeBus : ICrossThreadData {
    public LevelChangeState LevelChangeState { get; set; }
}

public enum LevelChangeState {
    Normal = 0,
    OldLevelUnloaded = 1,
    NewLevelLoaded = 2,
    Finished = 3,
}