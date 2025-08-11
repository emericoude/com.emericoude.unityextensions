using System;

using Emericoude.Attributes;

using UnityEngine;

namespace Emericoude.Tests
{
    public class CustomAttributesTests : MonoBehaviour
    {
        [BetterCurveField]
        [SerializeField] private AnimationCurve curve;
        
        [Layer]
        [SerializeField] private int layer;

        [RadiansDisplayedAsDegrees]
        [SerializeField] private float angle;

        [DrawInDebugInfoBox]
        [SerializeField] private float myFloat;
        [DrawInDebugInfoBox]
        [SerializeField] private string myString;
    }
}
