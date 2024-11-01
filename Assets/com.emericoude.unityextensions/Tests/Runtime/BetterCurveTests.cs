using Emericoude;
using UnityEngine;

public class BetterCurveTests : MonoBehaviour
{
    public AnimationCurve defaultCurve;

    [BetterCurveField]
    public AnimationCurve improvedCurve;

    [BetterCurveField( "Time", "Wow")] 
    public AnimationCurve improvedCurvePlus;
}
