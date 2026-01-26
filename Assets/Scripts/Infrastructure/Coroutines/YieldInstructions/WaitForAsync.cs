using System.Threading.Tasks;
using UnityEngine;

namespace FormForge.Coroutines.YieldInstructions
{
    public class WaitForAsync : CustomYieldInstruction
    {
        private Task m_task;

        /// <summary>
        /// The status of the <seealso cref="Task"/> we are waiting for
        /// </summary>
        public TaskStatus TaskStatus => m_task.Status;
        
        /// <summary>
        /// Waits for a running <seealso cref="Task"/> to complete, get cancelled, or fault.
        /// </summary>
        /// <param name="task">The task to wait for</param>
        public WaitForAsync(Task task)
        {
            m_task = task;
        }

        public override bool keepWaiting
        {
            get => !m_task.IsCompleted && 
                   !m_task.IsCanceled && 
                   !m_task.IsFaulted;
        }
    }
}