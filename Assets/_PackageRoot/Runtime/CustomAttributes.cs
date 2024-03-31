using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Emericoude.UnityExtensions
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class BetterCurveFieldAttribute : PropertyAttribute { }
}
