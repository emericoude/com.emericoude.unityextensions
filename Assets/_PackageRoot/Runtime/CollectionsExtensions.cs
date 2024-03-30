using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Emeric.Utilities
{
	public static class CollectionsExtensions
	{
		/// <summary> Gets a random element. </summary>
		/// <remarks> Consider using <see cref="GetRandomElement{T}(IEnumerable{T}, int)"/> instead, or your own solution as this uses Linq twice. </remarks>
		/// <returns> A random element in the collection. </returns>
		public static T GetRandomElement<T> (this IEnumerable<T> collection)
		{
			return collection.ElementAtOrDefault(Random.Range(0, collection.Count()));
		}

		/// <summary> Gets a random element. </summary>
		/// <remarks> Consider using a solution that fits your need, as this uses Linq. </remarks>
		/// <returns> A random element in the collection. </returns>
		public static T GetRandomElement<T>(this IEnumerable<T> collection, int count)
		{
			return collection.ElementAtOrDefault(Random.Range(0, count));
		}
	}
}
