// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace Extensions;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

public static class TaskExtensions {

    public async static Task WithCancellation(this Task task, CancellationToken cancellationToken) {

        var tcs = new TaskCompletionSource<bool>();

        // s is set when the cancellation happens,
        //      Meaning that WhenAny returns and the tcs.Task has been set, it'll throw the exception
        //      When task returns in WhenAny it is the result
        await using (cancellationToken.Register(callback: s => ((TaskCompletionSource<bool>)s!).TrySetResult(true), tcs)) {
            if (task != await Task.WhenAny(task, tcs.Task))
                throw new OperationCanceledException(cancellationToken);
        }


    }
}
