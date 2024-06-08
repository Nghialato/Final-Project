using System.Collections.Generic;
using System.Runtime.CompilerServices;
using _Scripts.Algorithm.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Algorithm.GenerateRoom
{
    public class RoomsRandomGenerate : GenerateRoomAbstract
    {
        [SerializeField] private RandomRoomData randomRoomData;
        public override void Generate(MapData mapData, ref int[,] logicMap, out List<RoomData> listRooms)
        {
            listRooms = new List<RoomData>();
            var numTries = randomRoomData.NumRoomsTriesInit;
            
            GetRangeRoomSize(mapData, out var minRoomSize, out var maxRoomSize);

            var numRooms = 0;
            for (var attempts = 0; attempts < numTries && numRooms <= mapData.numRoomsRequired; attempts++)
            {
                RandomRoomSize(minRoomSize, maxRoomSize, out var roomWidth, out var roomHeight);
                
                RandomRoomPos(mapData, roomWidth, roomHeight, out var xPos, out var yPos);

                // Check if room overlap
                var roomFits = true;
                for (var y = yPos - mapData.distanceBetweenRoom; y < yPos + roomHeight + mapData.distanceBetweenRoom; y++)
                {
                    for (var x = xPos - mapData.distanceBetweenRoom; x < xPos + roomWidth + mapData.distanceBetweenRoom; x++)
                    {
                        if(mapData.IsValidCell(x, y) == false) continue;
                        if (logicMap[x, y] == (int)MapType.None) continue;
                        roomFits = false;
                        break;
                    }
                    if (!roomFits)
                    {
                        break;
                    }
                }
                if (!roomFits) continue;
                
                // Add room in list and mark in Logic Map
                numRooms++;
                listRooms.Add(new RoomData(numRooms, new Vector2Int(xPos, yPos), roomWidth, roomHeight));
                for (var y = yPos; y < yPos + roomHeight; y++)
                {
                    for (var x = xPos; x < xPos + roomWidth; x++)
                    {
                        logicMap[x, y] = (int)MapType.Floor;
                    }
                }
            }
        }

        private void GetRangeRoomSize(in MapData mapData, out int minRoomSize, out int maxRoomSize)
        {
            var averageAreaRoom =
                mapData.mapSize.width * mapData.mapSize.height * mapData.percentFillMap / mapData.numRoomsRequired;

            var averageSize = (int)Mathf.Sqrt(averageAreaRoom);

            minRoomSize = averageSize - 2 - mapData.distanceBetweenRoom;
            maxRoomSize = averageSize + 2 - mapData.distanceBetweenRoom;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RandomRoomSize(in int minRoomSize, in int maxRoomSize, out int roomWidth, out int roomHeight)
        {
            roomWidth = Random.Range(minRoomSize, (maxRoomSize + 1));
            roomHeight = Random.Range(minRoomSize, (maxRoomSize + 1));

            roomWidth -= 1 - roomWidth % 2;
            roomHeight -= 1 - roomHeight % 2;
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RandomRoomPos(in MapData mapData, in int roomWidth, in int roomHeight, out int xPos, out int yPos)
        {
            xPos = Random.Range(2, mapData.mapSize.width - roomWidth - 2);
            yPos = Random.Range(2, mapData.mapSize.height - roomHeight - 2);

            xPos -= xPos % 2;
            yPos -= yPos % 2;
        }
    }
}