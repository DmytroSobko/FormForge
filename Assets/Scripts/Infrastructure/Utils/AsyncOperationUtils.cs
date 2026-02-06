using System.Threading.Tasks;
using UnityEngine;

namespace FormForge.Infrastructure.Utils
{
    public static class AsyncOperationUtils
    {
        public static Task AsTask(this AsyncOperation operation)
        {
            if (operation == null || operation.isDone)
            {
                return Task.CompletedTask;
            }

            var tcs = new TaskCompletionSource<bool>();

            operation.completed += _ => tcs.TrySetResult(true);

            return tcs.Task;
        }
    }
}