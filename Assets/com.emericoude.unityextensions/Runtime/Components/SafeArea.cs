using UnityEngine;

namespace Emericoude.UI
{
    /// <summary> Fills the "safe area" of a device's screen (for instance it avoids phone notches). </summary>
    /// <remarks> This happens within the constraint that it is given, so its position and size is relative its parent. </remarks>
    public class SafeArea : MonoBehaviour
    {
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            Canvas.ForceUpdateCanvases();
            FillRectToSafeArea();
        }

        private void OnDrawGizmos()
        {
            if (TryGetComponent(out _rectTransform))
            {
                FillRectToSafeArea();
                
                //Draw area
                UnityEngine.Gizmos.matrix = _rectTransform.localToWorldMatrix;
                UnityEngine.Gizmos.color = Color.magenta;
                UnityEngine.Gizmos.DrawWireCube(Vector3.zero, _rectTransform.rect.size);
            }
        }

        /// <summary>
        /// Makes the rect fill the safe area as much as it can. This is dependant on parenting.
        /// If you want it to full fill the safe area, ensure it is at the root of your canvas.
        /// </summary>
        public void FillRectToSafeArea()
        {
            Rect screenSafeArea = UnityEngine.Device.Screen.safeArea;
            Vector2 minAnchor = screenSafeArea.position;
            Vector2 maxAnchor = minAnchor + screenSafeArea.size;

            minAnchor.x /= UnityEngine.Device.Screen.width;
            minAnchor.y /= UnityEngine.Device.Screen.height;
            maxAnchor.x /= UnityEngine.Device.Screen.width;
            maxAnchor.y /= UnityEngine.Device.Screen.height;

            _rectTransform.anchorMin = minAnchor;
            _rectTransform.anchorMax = maxAnchor;
            _rectTransform.sizeDelta = Vector2.zero;
        }
    }
}
