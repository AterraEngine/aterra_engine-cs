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
    private readonly struct ReadReleaser(ReaderWriterLockSlim rwLock) : IDisposable {
        public void Dispose() => rwLock.ExitReadLock();
    }
    private readonly struct WriteReleaser(ReaderWriterLockSlim rwLock) : IDisposable {
        public void Dispose() => rwLock.ExitWriteLock();
    }
    private readonly struct UpgradableReadReleaser(ReaderWriterLockSlim rwLock) : IDisposable {
        public void Dispose() => rwLock.ExitUpgradeableReadLock();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IDisposable Read(this ReaderWriterLockSlim rwLock) {
        rwLock.EnterReadLock();
        return new ReadReleaser(rwLock);
    }

    public static IDisposable Write(this ReaderWriterLockSlim rwLock) {
        rwLock.EnterWriteLock();
        return new WriteReleaser(rwLock);
    }

    public static IDisposable UpgradeableRead(this ReaderWriterLockSlim rwLock) {
        rwLock.EnterUpgradeableReadLock();
        return new UpgradableReadReleaser(rwLock);
    }
    
    public static IDisposable? TryRead(this ReaderWriterLockSlim rwLock, int millisecondsTimeout, Action? onTimeout = null) {
        if (rwLock.TryEnterReadLock(millisecondsTimeout)) {
            return new ReadReleaser(rwLock);
        }
        onTimeout?.Invoke(); // Handle timeout scenario
        return null; 
    }

    public static IDisposable? TryWrite(this ReaderWriterLockSlim rwLock, int millisecondsTimeout, Action? onTimeout = null) {
        if (rwLock.TryEnterWriteLock(millisecondsTimeout)) {
            return new WriteReleaser(rwLock);
        }
        onTimeout?.Invoke(); // Handle timeout scenario
        return null; 
    }

    public static IDisposable? TryUpgradeableRead(this ReaderWriterLockSlim rwLock, int millisecondsTimeout, Action? onTimeout = null) {
        if (rwLock.TryEnterUpgradeableReadLock(millisecondsTimeout)) {
            return new UpgradableReadReleaser(rwLock);
        }
        onTimeout?.Invoke(); // Handle timeout scenario
        return null; 
    }
}