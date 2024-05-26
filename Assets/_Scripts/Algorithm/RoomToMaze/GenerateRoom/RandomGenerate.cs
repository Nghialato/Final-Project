using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Algorithm.GenerateRoom
{
    public class RandomGenerate : GenerateRoomAbstract
    {
        public override void Generate(RoomToMazeData roomToMazeData, ref int[,] logicMap, out List<RoomData> listRooms)
        {
            listRooms = new List<RoomData>();
            var averageAreaRoom =
                roomToMazeData.map.width * roomToMazeData.map.height * roomToMazeData.percentFillMap / roomToMazeData.numRoomsRequired;
            var numTries = roomToMazeData.numRoomsTriesInit > 0 ? roomToMazeData.numRoomsTriesInit : 10;

            var averageSize = (int)Mathf.Sqrt(averageAreaRoom);

            var minRoomSize = averageSize - 2 - roomToMazeData.distanceBetweenRoom;
            var maxRoomSize = averageSize + 2 - roomToMazeData.distanceBetweenRoom;

            var numRooms = 0;
            for (var attempts = 0; attempts < numTries && numRooms <= roomToMazeData.numRoomsRequired; attempts++)
            {
                var roomWidth = Random.Range(minRoomSize, (maxRoomSize + 1));
                var roomHeight = Random.Range(minRoomSize, (maxRoomSize + 1));

                roomWidth -= 1 - roomWidth % 2;
                roomHeight -= 1 - roomHeight % 2;
                
                var xPos = Random.Range(1, roomToMazeData.map.width - roomWidth - 1);
                var yPos = Random.Range(1, roomToMazeData.map.height - roomHeight - 1);

                xPos -= xPos % 2;
                yPos -= yPos % 2;

                var roomFits = true;
                for (var y = yPos - roomToMazeData.distanceBetweenRoom; y < yPos + roomHeight + roomToMazeData.distanceBetweenRoom; y++)
                {
                    for (var x = xPos - roomToMazeData.distanceBetweenRoom; x < xPos + roomWidth + roomToMazeData.distanceBetweenRoom; x++)
                    {
                        if(roomToMazeData.IsValidCell(x, y) == false) continue;
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
    }
}