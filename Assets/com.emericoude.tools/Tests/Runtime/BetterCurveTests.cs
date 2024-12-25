using Emericoude.Attributes;
using UnityEngine;

namespace Emericoude.Tests
{
    public class BetterCurveTests : MonoBehaviour
    {
        public AnimationCurve defaultCurve;

        [BetterCurveField]
        public AnimationCurve improvedCurve;

        [BetterCurveField( "Time", "Force")] 
        public AnimationCurve improvedCurvePlus;
    }
}
