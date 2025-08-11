using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Emericoude
{
    //TODO: IEquatable<Navigation3D>
    //TODO: Support cross-nav between screen-space and this by doing world to viewport raycasts
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

        [Tooltip("The navigation mode for directional control scheme (e.g. controller). Automatic uses sphere casting.")]
        public Mode NavigationMode;
        [Tooltip("The way we calculate what should be considered up/right/left/down. Generally, camera or local spaces are recommended.")]
        public AxisMode NavigationAxisMode;

        //Automatic nav settings
        [Tooltip("If true, this component will automatically setup the sphere cast settings used for navigation according to the button's collider bounds.")]
        public bool AutoSetupSphereCast;
        [Tooltip("The radius of the sphere cast.")]
        public float SphereCastRadius;
        [Tooltip("The maximum distance of the sphere cast.")]
        public float SphereCastMaximumDistance;
        [Tooltip("The maximum hit points of the sphere cast. User power of two if you can. The lower the better for performance, but might miss hits.")]
        public int SphereCastMaximumHits;
        [Tooltip("The layer of the sphere cast.")]
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
                    AutoSetupSphereCast = true,
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