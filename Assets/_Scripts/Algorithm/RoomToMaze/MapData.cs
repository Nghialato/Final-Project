using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Algorithm
{
    public static class MapDataEts
    {
        public static bool IsValidCell(this MapData mapData, int x, int y)
        {
            return x >= 0 && x < mapData.mapSize.width && y >= 0 && y < mapData.mapSize.height;
        }

        public static (int, int) PickStartPos(this MapData mapData, in int[,] logicMap)
        {
            var startX = Random.Range(0, mapData.mapSize.width - 2); 
            var startY = Random.Range(0, mapData.mapSize.height - 2); 
            if (startX % 2 == 1) startX++; 
            if (startY % 2 == 1) startY++; 
            
            while (logicMap[startX, startY] != (int)MapType.None)
            {
                startX = Random.Range(0, mapData.mapSize.width - 2); 
                startY = Random.Range(0, mapData.mapSize.height - 2); 
                if (startX % 2 == 1) startX++;
                if (startY % 2 == 1) startY++;
            }

            return (startX, startY);
        }
        
    }
    
    [Serializable]
    public class MapData
    {
        public MapSize mapSize;
        
        public int numRoomsRequired;
        
        public int distanceBetweenRoom;
    }

    [Serializable]
    public struct MapSize
    {
        public int width;
        public int height;
    }
}