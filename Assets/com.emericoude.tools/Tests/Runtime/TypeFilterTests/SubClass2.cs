using System;
using Sirenix.OdinInspector;

namespace Emericoude.Tests
{
    [Serializable]
    public class SubClass2 : BaseClass
    {
        public bool Sub2Bool = true;
        [ShowIf("@this.Sub2Bool == true")]
        public string SetToTrue = "Set Sub2Bool to true to see me";
    }
}