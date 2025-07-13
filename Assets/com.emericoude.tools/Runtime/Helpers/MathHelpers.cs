using UnityEngine;

namespace Emericoude
{
    public class MathHelpers : MonoBehaviour
    {
        public const float TAU = Mathf.PI * 2f;
        
        /// <returns> The value t (within the iMin and iMax range), remapped into the range oMin and oMax. <br/><c>Mathf.Lerp(oMin, oMax, Mathf.InverseLerp(iMin, iMax, t));</c> </returns>
        public static float Remap(float iMin, float iMax, float oMin, float oMax, float t) => Mathf.Lerp(oMin, oMax, Mathf.InverseLerp(iMin, iMax, t));
        /// <returns> The value t (within the iMin and iMax range), remapped into the range oMin and oMax. <br/><c>Mathf.LerpUnclamped(oMin, oMax, Mathf.InverseLerp(iMin, iMax, t));</c> </returns>
        public static float RemapUnclamped(float iMin, float iMax, float oMin, float oMax, float t) => Mathf.LerpUnclamped(oMin, oMax, Mathf.InverseLerp(iMin, iMax, t));

        //TODO: Test this, not sure why it's different from the vector one
        /// <returns> A bezier quadratic value. You can think of this as a lerp that outputs a Bézier curve (i.e. nested lerp between more than two points).
        /// <br/> Optimized version of <c>Mathf.Lerp(Mathf.Lerp(p0, p1, t), Mathf.Lerp(p1, p2, t), t);</c></returns>
        public static float QuadraticBezier(float p0, float p1, float p2, float t) {
            //return Mathf.Lerp(Mathf.Lerp(p0, p1, t), Mathf.Lerp(p1, p2, t), t);
            float u = 1f - t;
            float twoT = 2f * t;
            return p0 * u * u 
                 + p1 * twoT * u 
                 + p2 * twoT * twoT;
        }

        //TODO: Test this, not sure why it's different from the vector one
        /// <returns> A bezier quadratic value. You can think of this as a lerp that outputs a Bézier curve (i.e. nested lerp between more than two points).
        /// <br/> Optimized version of <c>Mathf.Lerp(MathHelpers.QuadraticBezier(p0, p1, p2, t), MathHelpers.QuadraticBezier(p1, p2, p3, t), t);</c></returns>
        public static float QuadraticBezier(float p0, float p1, float p2, float p3, float t) {
            //return Mathf.Lerp(QuadraticBezier(p0, p1, p2, t), QuadraticBezier(p1, p2, p3, t), t);
            float t3 = t * t * t;
            float threeT2 = 3f * t * t;
            float threeT = 3f * t;
            float threeT3 = 3f * t3;
            return p0 * (-t3 + threeT2 - threeT)
                 + p1 * (threeT3 - 2f * threeT2 + threeT)
                 + p2 * (-threeT3 + threeT2)
                 + p3 * (t3);
        }
    }
}
