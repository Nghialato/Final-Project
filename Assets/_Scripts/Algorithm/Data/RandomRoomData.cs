using UnityEngine;

namespace _Scripts.Algorithm.Data
{
    [CreateAssetMenu(fileName ="RandomRoomData",menuName = "PCG/RandomRoomData")]
    public class RandomRoomData : ScriptableObject
    {
        public int numRoomsTriesInit;

        public int NumRoomsTriesInit => numRoomsTriesInit > 0 ? numRoomsTriesInit : 10;
    }
}