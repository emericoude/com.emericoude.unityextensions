using System;
using Emericoude.Attributes;
using UnityEngine;

namespace Emericoude.Tests
{
    [Serializable]
    public class SubSubClass : SubClass
    {
        [BetterCurveField]
        public AnimationCurve SubSubAnimationCurve;
    }
}