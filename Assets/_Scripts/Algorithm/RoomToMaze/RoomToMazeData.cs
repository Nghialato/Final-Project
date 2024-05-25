using System;
using UnityEngine;

namespace _Scripts.Algorithm
{
    [Serializable]
    public class RoomToMazeData
    {
        public Map map;
        
        public int numRoomsTriesInit;

        public int numRoomsRequired;

        [Range(0, 1)]
        public float percentFillMap;
        
        [Range(0, 100)]
        public int extraConnectorChance;

        [Range(0, 100)]
        public int imperfectRate;

        // Todo Set up data for room
    }

    [Serializable]
    public struct Map
    {
        public int width;
        public int height;
    }
}