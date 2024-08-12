// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Pools;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ReaderWriterLockSlimExtensions {
    public static IDisposable ReadLock(this ReaderWriterLockSlim rwLock) => new ReadLockDisposable(rwLock);
    public static IDisposable WriteLock(this ReaderWriterLockSlim rwLock) => new WriteLockDisposable(rwLock);
    public static IDisposable UpgradeableReadLock(this ReaderWriterLockSlim rwLock) => new UpgradeableReadLockDisposable(rwLock);

    private readonly struct ReadLockDisposable : IDisposable {
        private readonly ReaderWriterLockSlim _rwLock;

        public ReadLockDisposable(ReaderWriterLockSlim rwLock) {
            _rwLock = rwLock;
            rwLock.EnterReadLock();
        }

        public void Dispose() {
            _rwLock.ExitReadLock();
        }
    }

    private readonly struct UpgradeableReadLockDisposable : IDisposable {
        private readonly ReaderWriterLockSlim _rwLock;

        public UpgradeableReadLockDisposable(ReaderWriterLockSlim rwLock) {
            _rwLock = rwLock;
            rwLock.EnterUpgradeableReadLock();
        }

        public void Dispose() {
            _rwLock.ExitUpgradeableReadLock();
        }
    }

    private readonly struct WriteLockDisposable : IDisposable {
        private readonly ReaderWriterLockSlim _rwLock;

        public WriteLockDisposable(ReaderWriterLockSlim rwLock) {
            _rwLock = rwLock;
            rwLock.EnterWriteLock();
        }

        public void Dispose() {
            _rwLock.ExitWriteLock();
        }
    }
}