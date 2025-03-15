using System;
using UnityEngine.EventSystems;

namespace Emericoude
{
    //TODO: IEquatable<Navigation3D>
    public struct Navigation3D
    {
        [Flags]
        public enum Mode
        {
            None = 0,
            Horizontal = 1,
            Vertical = 2,
            Automatic = 3,
            Explicit = 4
        }

        public Mode NavigationMode;
        public bool WrapAround;
        
        //Automatic nav settings
        //TODO: raycast? cache a list? do angle checks?
        
        //Explicit nav
        public ISelectHandler SelectOnUp;
        public ISelectHandler SelectOnDown;
        public ISelectHandler SelectOnLeft;
        public ISelectHandler SelectOnRight;

        public static Navigation3D DefaultNavigation3D
        {
            get
            {
                var defaultNav = new Navigation3D();
                defaultNav.NavigationMode = Mode.Automatic;
                defaultNav.WrapAround = false;
                return defaultNav;
            }
        }
    }
}