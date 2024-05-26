using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace _Scripts.GameEts
{
    public static class GameEts
    {
        public static void Shuffle<T>(this List<T> array)
        {
            var n = array.Count;
            while (n > 1)
            {
                var k = Random.Range(0, n);
                (array[k], array[n - 1]) = (array[n - 1], array[k]);
                n--;
            }
        }
    }
}