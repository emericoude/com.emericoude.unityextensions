using System.Collections.Generic;
using UnityEngine;
#if CYSHARP_ZLINQ
using ZLinq;
#endif

namespace Emericoude
{
    /// <summary>
    /// Helpers for the ZLinq library. You can find it here: https://github.com/Cysharp/ZLinq?tab=readme-ov-file#unity
    /// <para/> This tutorial by git-amend may also be useful: https://www.youtube.com/watch?v=gX5nD2LeAvQ
    /// </summary>
    public static class ZLinqHelpers
    {
        #if CYSHARP_ZLINQ
        
        /// <summary>Re-converts a ValueEnumerable to a regular Enumerable.</summary>
        /// <remarks>
        /// This does have to do some allocation, but can be useful in some scenarios such as the following example.
        /// In this example, converting back to an Enumerable using AsEnumerable allows us to print each value. <para/>
        ///     private static readonly int[] source = new int[] { 0, 1, 2, 3, 4, 5 }; <br/>
        ///     var result = source.AsValueEnumerable().Where(static x => x % 2 == 0).Select(static x => x * 3); <br/>
        ///     Debug.Log(string.Join(", ", result.AsEnumerable()));
        /// </remarks>
        /// <returns>The ValueEnumerable as an Enumerable.</returns>
        public static IEnumerable<T> AsEnumerable<TEnumerator, T>(this ValueEnumerable<TEnumerator, T> valueEnumerable) where TEnumerator : struct, IValueEnumerator<T> 
        {
            using (var enumerator = valueEnumerable.Enumerator)
            {
                while (enumerator.TryGetNext(out var current))
                {
                    yield return current;
                }
            }
        }
        
        #endif
    }
}
