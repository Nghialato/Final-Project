using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Algorithm
{
    public class RoomToMazeAlgorithm : AbstractDungeonGenerator
    {
        public RoomToMazeData roomToMazeData;

        private int[,] _logicMap;
        
        protected override void RunProceduralGeneration()
        {
            HashSet<Vector2Int> floor = new();

            SizeMapValidation();
            
            _logicMap = new int[roomToMazeData.map.width, roomToMazeData.map.height];
            
            for (int i = 0; i < roomToMazeData.map.width; i++)
            {
                for (int j = 0; j < roomToMazeData.map.height; j++)
                {
                    _logicMap[i, j] = (int)MapType.None;
                }
            }
            
            GenRoomInMap();

            // GenMazeInMap();
            //
            // ConnectRegion();
            //
            // RemoveDeadEnds();

            for (int i = 0; i < roomToMazeData.map.width; i++)
            {
                for (int j = 0; j < roomToMazeData.map.height; j++)
                {
                    if (_logicMap[i, j] == (int)MapType.Floor)
                    {
                        floor.Add(new Vector2Int(i, j));
                    }
                }
            }
            
            tilemapVisualizer.PaintFloorTiles(floor);
            // WallGenerator.CreateWalls(floor, tilemapVisualizer);
        }

        private void SizeMapValidation()
        {
            if (roomToMazeData.map.width % 2 == 0)
            {
                roomToMazeData.map.width++;
            }
            
            if (roomToMazeData.map.height % 2 == 0)
            {
                roomToMazeData.map.height++;
            }
        }

        private void GenRoomInMap()
        {
            var averageAreaRoom =
                roomToMazeData.map.width * roomToMazeData.map.height * roomToMazeData.percentFillMap / roomToMazeData.numRoomsRequired;
            var numTries = roomToMazeData.numRoomsTriesInit > 0 ? roomToMazeData.numRoomsTriesInit : 10;

            var averageSize = (int)Mathf.Sqrt(averageAreaRoom);

            var minRoomSize = (int)(averageSize - 2);
            var maxRoomSize = (int)(averageSize + 2);

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
                for (var y = yPos; y < yPos + roomHeight; y++)
                {
                    for (var x = xPos; x < xPos + roomWidth; x++)
                    {
                        if (_logicMap[x, y] == (int)MapType.None) continue; // Assuming 0 represents empty space
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
                for (var y = yPos; y < yPos + roomHeight; y++)
                {
                    for (var x = xPos; x < xPos + roomWidth; x++)
                    {
                        _logicMap[x, y] = (int)MapType.Floor;
                    }
                }
            }
        }
        
        private void GenMazeInMap()
        {
            
        }

        private void ConnectRegion()
        {
            
        }

        private void RemoveDeadEnds()
        {
            
        }

    }

    internal enum MapType
    {
        None, Floor, Wall
    }
    
}
