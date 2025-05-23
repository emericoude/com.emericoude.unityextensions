using Emericoude.Helpers;

using Sirenix.OdinInspector;

using UnityEngine;

namespace Emericoude.Tests
{
    public class Test_Curve_GetTimeFromValue : MonoBehaviour
    {
        public float value = 0f;
        public AnimationCurve curve;
        public AnimationCurve inverseCurve;
        public int accuracy = 16;
        

        [Button]
        public void PrintCurveTimeFromValue() {
            Debug.Log($"Time at value {this.value} is {this.curve.GetTimeFromValue(this.value, this.accuracy)}");
        }
    }
}
