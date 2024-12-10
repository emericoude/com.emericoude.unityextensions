using Emericoude.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Emericoude.UI
{
    //TODO: Clean up different methods into their own code blocks
    //TODO: Explore resizing options (fit-to-children and fit-children-to-size)
    //TODO: Explore controlling children pivot relative to ring
    //TODO: Mirroring option (or "bidirectional inverse)
    //TODO: Find a better name for everything
    
    public class RadialLayoutGroup : LayoutGroup
    {
        public enum RadialLayoutMethod
        {
            EvenDistribution,
            AngleDistribution
        }

        public enum RadialDirection
        {
            Clockwise,
            Counterclockwise,
            Bidirectional
        }

        [Header("Radial Layout Group")]
        public RadialLayoutMethod layoutMethod = RadialLayoutMethod.EvenDistribution;
        public RadialDirection layoutDirection = RadialDirection.Clockwise;
        [Range(0f, 1f)] public float startAngle = 0f;
        [Range(0f, 1f)] public float radius = 1f;
        public bool rotateTowardsCenter = false;
        
        [Header("Angle Distribution")]
        [Range(0f, 360f)] public float angleDistribution = 30f;

        private void OnDrawGizmos()
        {
            foreach (Transform child in rectTransform)
            {
                UnityEngine.Gizmos.DrawLine(rectTransform.position, child.position);
            }
        }

        public override void CalculateLayoutInputVertical()
        {

        }

        public override void SetLayoutHorizontal()
        {
            SetChildPositions();
        }

        public override void SetLayoutVertical()
        {
            SetChildPositions();
        }

        private void SetChildPositions()
        {
            int childCount = rectChildren.Count;
            int oddNumberIndex = 1;
            float oddNumberInterval = 1f / (childCount / 2f);
            float startAngleDegrees = startAngle * Mathf.PI * 2f;
            for (int i = 0; i < childCount; i++)
            {
                bool isEven = i % 2 == 0;
                float angle = layoutMethod switch { 
                    RadialLayoutMethod.EvenDistribution => ((360f / childCount) * i) * Mathf.Deg2Rad,
                    RadialLayoutMethod.AngleDistribution => angleDistribution * i * Mathf.Deg2Rad,
                    _ => 0f
                };
                
                angle *= layoutDirection switch {
                    RadialDirection.Clockwise => 1f,
                    RadialDirection.Counterclockwise => -1f,
                    RadialDirection.Bidirectional => isEven ? 0.5f : 1f,
                    _ => 1f
                };
                
                if (layoutDirection == RadialDirection.Bidirectional && !isEven)
                {
                    if (layoutMethod == RadialLayoutMethod.AngleDistribution)
                    {
                        angle = -angleDistribution * (i + 1) * Mathf.Deg2Rad * 0.5f;
                    }
                    else
                    {
                        angle -= Mathf.PI * ((i + oddNumberIndex) * oddNumberInterval);
                    }
                    oddNumberIndex++;
                }
                
                angle += startAngleDegrees;

                float pixelRadius = (rectTransform.rect.size.SmallestComponent() / 2f) * radius;
                Vector2 pos = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * pixelRadius;

                RectTransform child = this.rectChildren[i];
                child.anchorMin = child.anchorMax = child.pivot = new Vector2(0.5f, 0.5f);
                child.anchoredPosition = pos;

                if (rotateTowardsCenter) {
                    float rotationAngle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg - 90f;
                    child.localRotation = Quaternion.Euler(0, 0, rotationAngle);
                }
                
                m_Tracker.Add(this, child, 
                    DrivenTransformProperties.Anchors |
                    DrivenTransformProperties.AnchoredPosition |
                    DrivenTransformProperties.Pivot |
                    DrivenTransformProperties.Rotation
                );
            }
        }
    }
}
