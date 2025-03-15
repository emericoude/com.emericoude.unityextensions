using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Emericoude
{
    //TODO: IEquatable<Navigation3D>
    [Serializable]
    public struct Navigation3D
    {
        public enum Mode
        {
            None = 0,
            Horizontal = 1,
            Vertical = 2,
            Automatic = 3,
            Explicit = 4
        }

        public enum AxisMode
        {
            WorldSpace = 0,
            LocalSpace = 1,
            CameraSpace = 2
        }

        public Mode NavigationMode;
        public AxisMode NavigationAxisMode;

        //Automatic nav settings
        public bool AutomateSphereCastCalculationFromColliderBounds;
        public float SphereCastRadius;
        public float SphereCastMaximumDistance;
        public int SphereCastMaximumHits;
        public LayerMask SphereCastLayer;

        //Explicit nav
        public GameObject SelectOnUp;
        public GameObject SelectOnDown;
        public GameObject SelectOnLeft;
        public GameObject SelectOnRight;

        public static Navigation3D DefaultNavigation
        {
            get {
                var defaultNav = new Navigation3D
                {
                    NavigationMode = Mode.Automatic,
                    NavigationAxisMode = AxisMode.CameraSpace,
                    AutomateSphereCastCalculationFromColliderBounds = true,
                    SphereCastLayer = ~0,
                    SphereCastMaximumHits = 16,
                };
                return defaultNav;
            }
        }
        
        public GameObject GetExplicitNavigation(AxisEventData axisEventData)
        {
            return axisEventData.moveDir switch
            {
                MoveDirection.Left => this.SelectOnLeft,
                MoveDirection.Up => this.SelectOnUp,
                MoveDirection.Right => this.SelectOnRight,
                MoveDirection.Down => this.SelectOnDown,
                MoveDirection.None => null,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}