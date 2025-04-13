using System.Collections.Generic;
using UnityEngine;

#if CYSHARP_ZLINQ
using ZLinq;
#else
using System.Linq;
#endif

namespace Emericoude.Helpers
{
	public static class CollectionsHelpers
	{
		/// <summary> Gets a random element. </summary>
		/// <remarks> Consider using <see cref="GetRandomElement{T}(IEnumerable{T}, int)"/> instead, or your own solution as this uses Linq twice (unless you have ZLinq). </remarks>
		/// <returns> A random element in the collection. </returns>
		public static T GetRandomElement<T> (this IEnumerable<T> collection)
		{
			#if CYSHARP_ZLINQ
			var collectionAsValueEnumerable = collection.AsValueEnumerable();
			return collectionAsValueEnumerable.ElementAtOrDefault(Random.Range(0, collectionAsValueEnumerable.Count()));
			#else
			return collection.ElementAtOrDefault(Random.Range(0, collection.Count()));
			#endif
		}

		/// <summary> Gets a random element. </summary>
		/// <remarks> Consider using a solution that fits your need, as this uses Linq (unless you have ZLinq). </remarks>
		/// <returns> A random element in the collection. </returns>
		public static T GetRandomElement<T>(this IEnumerable<T> collection, int enumerableCount)
		{
			#if CYSHARP_ZLINQ
			return collection.AsValueEnumerable().ElementAtOrDefault(Random.Range(0, enumerableCount));
			#else
			return collection.ElementAtOrDefault(Random.Range(0, enumerableCount));
			#endif
		}
	}
}
