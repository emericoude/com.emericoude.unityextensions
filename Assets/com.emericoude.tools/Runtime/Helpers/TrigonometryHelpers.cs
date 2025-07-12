using UnityEngine;

namespace Emericoude.Helpers
{
    public static class TrigonometryHelpers
    {
        /// <returns> The angle (in radians) of the direction relative to Vector2.right (counterclockwise). </returns>
        /// <remarks> Directions are expected to be normalized. </remarks>
        public static float DirectionToAngle(float x, float y) => Mathf.Atan2(y, x);
        /// <returns> The angle (in radians) of the direction relative to Vector2.right (counterclockwise). </returns>
        /// <remarks> Directions are expected to be normalized. </remarks>
        public static float DirectionToAngle(this Vector2 direction) => Mathf.Atan2(direction.y, direction.x);
        
        /// <returns> A direction equivalent to the angle (in radians), where Vector2.right is 0. </returns>
        public static Vector2 AngleToDirection(float angleRad) => new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        
        //the clamp is only necessary because of floating point errors
        /// <returns> The angle (in radians) of the "to" direction relative to the "from" direction (counterclockwise). </returns>
        /// <remarks> Directions are expected to be normalized. </remarks>
        public static float AngleBetween(this Vector2 from, Vector2 to) => Mathf.Acos( Mathf.Clamp(Vector3.Dot(from, to), -1f, 1f)); 
        /// <returns> The angle (in radians) of the "to" direction relative to the "from" direction (counterclockwise). </returns>
        /// <remarks> Directions are expected to be normalized. </remarks>
        public static float AngleBetween(this Vector3 from, Vector3 to) => Mathf.Acos(Mathf.Clamp(Vector3.Dot(from, to), -1f, 1f));
        
    }
}
