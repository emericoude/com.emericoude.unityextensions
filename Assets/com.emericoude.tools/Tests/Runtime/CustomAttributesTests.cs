using System;

using UnityEngine;

namespace Emericoude.Tests
{
    public class CustomAttributesTests : MonoBehaviour
    {
        [Layer]
        [SerializeField] private int layer;

        [RadiansDisplayedAsDegrees]
        [SerializeField] private float angle;
    }
}
