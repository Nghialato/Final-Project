using UnityEngine;

namespace _Scripts.Algorithm.Data
{
    [CreateAssetMenu(fileName ="FloodFillGenerateData",menuName = "PCG/FloodFillGenerateData")]
    public class FloodFillGenerateData : ScriptableObject
    {
        [Range(0, 100)]
        public int percentChangeDirection;
    }
}