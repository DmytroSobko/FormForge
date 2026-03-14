using FormForge.Infrastructure.UI.Toast;
using UnityEngine;

namespace FormForge.ScriptableObjects.Visuals.Toasts
{
    [CreateAssetMenu(fileName = "ToastVisualsConfig", menuName = "Scriptable Objects/Visuals/ToastVisualsConfig")]
    public class ToastVisualsConfig : ScriptableObject
    {
        public EToastType Type;
        public Sprite Icon;
    }
}