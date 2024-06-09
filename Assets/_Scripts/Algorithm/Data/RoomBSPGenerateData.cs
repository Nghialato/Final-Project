using UnityEngine;

namespace _Scripts.Algorithm.Data
{
    [CreateAssetMenu(fileName ="RoomBSPGenerateData",menuName = "PCG/RoomBSPGenerateData")]
    public class RoomBSPGenerateData : ScriptableObject
    {
        [Range(0, 100)] public int percentSplitHorizontal;
    }
}