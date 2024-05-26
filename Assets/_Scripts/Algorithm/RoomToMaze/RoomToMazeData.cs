using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Algorithm
{
    public static class RoomToMazeDataEts
    {
        public static bool IsValidCell(this RoomToMazeData roomToMazeData, int x, int y)
        {
            return x >= 0 && x < roomToMazeData.map.width && y >= 0 && y < roomToMazeData.map.height;
        }

        public static (int, int) PickStartPos(this RoomToMazeData roomToMazeData, in int[,] logicMap)
        {
            var startX = Random.Range(0, roomToMazeData.map.width - 2); 
            var startY = Random.Range(0, roomToMazeData.map.height - 2); 
            if (startX % 2 == 1) startX++; 
            if (startY % 2 == 1) startY++; 
            
            while (logicMap[startX, startY] != (int)MapType.None)
            {
                startX = Random.Range(0, roomToMazeData.map.width - 2); 
                startY = Random.Range(0, roomToMazeData.map.height - 2); 
                if (startX % 2 == 1) startX++;
                if (startY % 2 == 1) startY++;
            }

            return (startX, startY);
        }
        
    }
    
    [Serializable]
    public class RoomToMazeData
    {
        public Map map;
        
        public int numRoomsTriesInit;

        public int numRoomsRequired;

        [Range(0, 1)]
        public float percentFillMap;

        [Range(0, 100)] public int chanceToChangePortal;

        [Range(0, 100)]
        public int imperfectRate;
        
        [Range(0, 100)]
        public int percentChangeDirection;

        [Range(0, 100)] public int removeMazeRate;

        public int distanceBetweenRoom;
        public int distanceBetweenRoomAndPath = 4;

        // Todo Set up data for room
    }

    [Serializable]
    public struct Map
    {
        public int width;
        public int height;
    }
}