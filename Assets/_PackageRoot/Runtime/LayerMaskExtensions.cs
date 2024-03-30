using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Emericoude.UnityExtensions
{
	public static class LayerMaskExtensions
	{
		/// <returns> True if <paramref name="layerMask"/> contains <paramref name="layerIndex"/>; otherwise false. </returns>
		public static bool Contains(this LayerMask layerMask, int layerIndex)
		{
			return layerMask == (layerMask | (1 << layerIndex));
		}

		/// <summary> Convert the layermask to a layer index. </summary>
		/// <returns> The layer index for this layer mask. </returns>
		public static int ToSingleLayerIndex(this LayerMask layerMask)
		{
			return (int)Mathf.Log(layerMask.value, 2);
		}
	}
}
