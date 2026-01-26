using System.Collections;

namespace FormForge.UpdateService.Interfaces
{
    public interface IDelayedInvoker
    {
        IEnumerator Delay();
    }
}