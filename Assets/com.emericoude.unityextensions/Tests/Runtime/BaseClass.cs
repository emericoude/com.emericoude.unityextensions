using System;
using Emericoude.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public abstract class BaseClass
{
    public float BaseFloat = 2;
}

[Serializable]
public class SubClass : BaseClass
{
    public string SubString = "Wow";
}

[Serializable]
public class SubSubClass : SubClass
{
    [BetterCurveField]
    public AnimationCurve SubSubAnimationCurve;
}

[Serializable]
public class SubClass2 : BaseClass
{
    public bool Sub2Bool = true;
    [ShowIf("@this.Sub2Bool == true")]
    public string SetToTrue = "Set Sub2Bool to true to see me";
}
