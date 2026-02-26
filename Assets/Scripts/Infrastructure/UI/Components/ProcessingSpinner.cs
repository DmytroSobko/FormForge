using UnityEngine;

namespace FormForge.Infrastructure.UI.Components
{
    public class ProcessingSpinner : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 360f;

        private void Update()
        {
            transform.Rotate(0f, 0f, -rotationSpeed * Time.unscaledDeltaTime);
        }
    }
}