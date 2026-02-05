using System.Threading.Tasks;
using UnityEngine;

namespace FormForge.Utils
{
    public static class AsyncOperationUtils
    {
        public static Task AsTask(this AsyncOperation operation)
        {
            if (operation.isDone)
            {
                return Task.CompletedTask;
            }

            var tcs = new TaskCompletionSource<bool>();

            operation.completed += _ => tcs.TrySetResult(true);

            return tcs.Task;
        }
    }
}