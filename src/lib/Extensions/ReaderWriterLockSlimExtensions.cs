// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ReaderWriterLockSlimExtensions {
    // -----------------------------------------------------------------------------------------------------------------
    // Helper Class
    // -----------------------------------------------------------------------------------------------------------------
    private class Releaser(Action releaseAction) : IDisposable {
        public void Dispose() => releaseAction();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IDisposable Read(this ReaderWriterLockSlim rwLock) {
        rwLock.EnterReadLock();
        return new Releaser(rwLock.ExitReadLock);
    }

    public static IDisposable Write(this ReaderWriterLockSlim rwLock) {
        rwLock.EnterWriteLock();
        return new Releaser(rwLock.ExitWriteLock);
    }

    public static IDisposable UpgradeableRead(this ReaderWriterLockSlim rwLock) {
        rwLock.EnterUpgradeableReadLock();
        return new Releaser(rwLock.ExitUpgradeableReadLock);
    }
    
    public static IDisposable? TryRead(this ReaderWriterLockSlim rwLock, int millisecondsTimeout, Action? onTimeout = null) {
        if (rwLock.TryEnterReadLock(millisecondsTimeout)) {
            return new Releaser(rwLock.ExitReadLock);
        }
        onTimeout?.Invoke(); // Handle timeout scenario
        return null; 
    }

    public static IDisposable? TryWrite(this ReaderWriterLockSlim rwLock, int millisecondsTimeout, Action? onTimeout = null) {
        if (rwLock.TryEnterWriteLock(millisecondsTimeout)) {
            return new Releaser(rwLock.ExitWriteLock);
        }
        onTimeout?.Invoke(); // Handle timeout scenario
        return null; 
    }

    public static IDisposable? TryUpgradeableRead(this ReaderWriterLockSlim rwLock, int millisecondsTimeout, Action? onTimeout = null) {
        if (rwLock.TryEnterUpgradeableReadLock(millisecondsTimeout)) {
            return new Releaser(rwLock.ExitUpgradeableReadLock);
        }
        onTimeout?.Invoke(); // Handle timeout scenario
        return null; 
    }
}