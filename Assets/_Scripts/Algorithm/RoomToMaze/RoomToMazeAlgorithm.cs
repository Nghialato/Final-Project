using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.GameEts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Algorithm
{
    public class RoomToMazeAlgorithm : AbstractDungeonGenerator
    {
        public RoomToMazeData roomToMazeData;

        private int[,] _logicMap;

        private Queue<Vector2Int> _mazeQueue = new();

        [SerializeField] private List<RoomData> _listRooms = new();
        private Queue<Vector2Int> _deadEnds;

        [Serializable]
        private class RoomData
        {
            public int roomId;
            public Vector2Int roomPos;
            public int width;
            public int height;
            public bool isConnectted = false;
            public List<Vector2Int> listPortals = new ();

            public RoomData(int roomId, Vector2Int roomPos, int width, int height)
            {
                this.roomId = roomId;
                this.roomPos = roomPos;
                this.width = width;
                this.height = height;
                listPortals.Clear();
            }

            public bool IsValidPortal(int i, int j)
            {
                var validPos = (i >= roomPos.x && i <= roomPos.x + width) &&
                               (j >= roomPos.y && j <= roomPos.y + height);
                
                return validPos;
            }

            public bool IsHadPortalSameSide(int i, int j)
            {
                foreach (var portal in listPortals)
                {
                    if (portal.x == i || portal.y == j)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
        
        protected override void RunProceduralGeneration()
        {
            HashSet<Vector2Int> floor = new();
            HashSet<Vector2Int> maze = new();
            HashSet<Vector2Int> dot = new();
            _mazeQueue = new();
            _listRooms.Clear();
            _deadEnds = new Queue<Vector2Int>();
            
            SizeMapValidation();
            
            _logicMap = new int[roomToMazeData.map.width, roomToMazeData.map.height];
            
            for (var i = 0; i < roomToMazeData.map.width; i++)
            {
                for (var j = 0; j < roomToMazeData.map.height; j++)
                {
                    _logicMap[i, j] = (int)MapType.None;
                }
            }
            
            GenRoomInMap();

            GenMazeInMap();
            
            ConnectRooms();
            
            RemoveDeadEnds();

            for (int i = 0; i < roomToMazeData.map.width; i++)
            {
                for (int j = 0; j < roomToMazeData.map.height; j++)
                {
                    if (_logicMap[i, j] == (int)MapType.Floor)
                    {
                        floor.Add(new Vector2Int(i, j));
                    }
                    
                    if (_logicMap[i, j] == (int)MapType.Maze)
                    {
                        maze.Add(new Vector2Int(i, j));
                    }
                    
                    if (_logicMap[i, j] == (int)MapType.Dot)
                    {
                        dot.Add(new Vector2Int(i, j));
                    }
                }
            }

            RunBuildMap(floor, maze, dot);
        }

        private async Task RunBuildMap(HashSet<Vector2Int> floor, HashSet<Vector2Int> maze, HashSet<Vector2Int> dot)
        {
            foreach (var room in _listRooms)
            {
                foreach (var portal in room.listPortals)
                {
                    _mazeQueue.Enqueue(portal);
                }
            }
            tilemapVisualizer.PaintFloorTiles(floor);
            tilemapVisualizer.PaintMazeTiles(maze);
            await tilemapVisualizer.PaintDotTilesAsync(dot);
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

            var minRoomSize = averageSize - 2;
            var maxRoomSize = averageSize + 2;

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
                        if (_logicMap[x, y] == (int)MapType.None) continue;
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
                _listRooms.Add(new RoomData(numRooms, new Vector2Int(xPos, yPos), roomWidth, roomHeight));
                for (var y = yPos; y < yPos + roomHeight; y++)
                {
                    for (var x = xPos; x < xPos + roomWidth; x++)
                    {
                        _logicMap[x, y] = (int)MapType.Floor;
                    }
                }
            }
        }

        #region Gen Maze

        private void GenMazeInMapFloodFill()
        {
            var startPos = PickStartPos();

            _logicMap[startPos.Item1, startPos.Item2] = (int)MapType.Maze;

            var queue = new Queue<(Vector2Int, Vector2Int)>();
            queue.Enqueue((new Vector2Int(startPos.Item1, startPos.Item2), Vector2Int.up));
            _mazeQueue.Enqueue(new Vector2Int(startPos.Item1, startPos.Item2));

            var directions = new [] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                var direction = current.Item2;
                
                if(Random.Range(0, 100) < roomToMazeData.percentChangeDirection) 
                {
                    // Change Direction
                    directions.Shuffle();
                    direction = direction == directions[0] ? directions[1] : directions[0];
                }
                var neighbor = current.Item1 + direction * 2;

                if (IsValidCell(neighbor.x, neighbor.y) && _logicMap[neighbor.x, neighbor.y] == (int)MapType.None)
                {
                    _logicMap[neighbor.x, neighbor.y] = (int)MapType.Maze;
                    _logicMap[neighbor.x - direction.x, neighbor.y - direction.y] = (int)MapType.Maze;
                    queue.Enqueue((neighbor, direction));
                    _mazeQueue.Enqueue(new Vector2Int(neighbor.x - direction.x, neighbor.y - direction.y));
                    _mazeQueue.Enqueue(new Vector2Int(neighbor.x, neighbor.y));
                }
                else
                {
                    foreach (var dir in directions)
                    {
                        var nei = current.Item1 + dir * 2;

                        if (IsValidCell(nei.x, nei.y) && _logicMap[nei.x, nei.y] == (int)MapType.None)
                        {
                            _logicMap[nei.x, nei.y] = (int)MapType.Maze;
                            _logicMap[nei.x - dir.x, nei.y - dir.y] = (int)MapType.Maze;
                            queue.Enqueue((nei, dir));
                            _mazeQueue.Enqueue(new Vector2Int(nei.x - dir.x, nei.y - dir.y));
                            _mazeQueue.Enqueue(new Vector2Int(nei.x, nei.y));
                        }
                    }
                }
            }
        }
        
        private void GenMazeInMap()
        {
            var percentChangeDirection = 40;

            var startPos = PickStartPos();

            _logicMap[startPos.Item1, startPos.Item2] = (int)MapType.Maze;

            var queue = new Queue<(Vector2Int, Vector2Int)>();
            queue.Enqueue((new Vector2Int(startPos.Item1, startPos.Item2), Vector2Int.up));
            _mazeQueue.Enqueue(new Vector2Int(startPos.Item1, startPos.Item2));

            var directions = new [] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};

            var possibleMoveFromStartPos = new Queue<(Vector2Int, Vector2Int)>();

            while (queue.Count > 0 || possibleMoveFromStartPos.Count > 0)
            {
                if (queue.Count == 0)
                {
                    while (possibleMoveFromStartPos.Count > 0)
                    {
                        var position = possibleMoveFromStartPos.Dequeue();
                        var nextStep = position.Item1 + position.Item2;
                        if (IsValidCell(nextStep.x, nextStep.y) && _logicMap[nextStep.x, nextStep.y] == (int)MapType.None)
                        {
                            queue.Enqueue((position.Item1, position.Item2));
                            while (possibleMoveFromStartPos.Peek().Item1 == position.Item1)
                            {
                                possibleMoveFromStartPos.Dequeue();
                            }
                            break;
                        }
                    }

                    if (possibleMoveFromStartPos.Count == 0) break;
                }
                var current = queue.Dequeue();

                var direction = current.Item2;
                
                // Add Possible Move
                
                foreach (var dir in directions)
                {
                    var check = current.Item1 + dir * 2;

                    if (IsValidCell(check.x, check.y) && _logicMap[check.x, check.y] == (int)MapType.None)
                    {
                        possibleMoveFromStartPos.Enqueue((current.Item1, dir));
                    }
                }
                
                if(Random.Range(0, 100) < percentChangeDirection) 
                {
                    // Change Direction
                    directions.Shuffle();
                    direction = direction == directions[0] ? directions[1] : directions[0];
                }

                var neighbor = current.Item1 + direction * 2;

                if (IsValidCell(neighbor.x, neighbor.y) && _logicMap[neighbor.x, neighbor.y] == (int)MapType.None)
                {
                    _logicMap[neighbor.x, neighbor.y] = (int)MapType.Maze;
                    _logicMap[neighbor.x - direction.x, neighbor.y - direction.y] = (int)MapType.Maze;
                    queue.Enqueue((neighbor, direction));
                    _mazeQueue.Enqueue(new Vector2Int(neighbor.x - direction.x, neighbor.y - direction.y));
                    _mazeQueue.Enqueue(new Vector2Int(neighbor.x, neighbor.y));
                }
                else
                {
                    var haveDirectionMove = false;
                    foreach (var dir in directions)
                    {
                        neighbor = current.Item1 + dir * 2;

                        if (IsValidCell(neighbor.x, neighbor.y) && _logicMap[neighbor.x, neighbor.y] == (int)MapType.None)
                        {
                            if (haveDirectionMove == false)
                            {
                                _logicMap[neighbor.x, neighbor.y] = (int)MapType.Maze;
                                _logicMap[neighbor.x - dir.x, neighbor.y - dir.y] = (int)MapType.Maze;
                                queue.Enqueue((neighbor, dir));
                                _mazeQueue.Enqueue(new Vector2Int(neighbor.x - dir.x, neighbor.y - dir.y));
                                _mazeQueue.Enqueue(new Vector2Int(neighbor.x, neighbor.y));
                                haveDirectionMove = true;
                                continue;
                            }
                            possibleMoveFromStartPos.Enqueue((current.Item1, dir));
                        }
                    }
                }
            }
        }

        private (int, int) PickStartPos()
        {
            var startX = Random.Range(0, roomToMazeData.map.width - 2); 
            var startY = Random.Range(0, roomToMazeData.map.height - 2); 
            if (startX % 2 == 1) startX++; 
            if (startY % 2 == 1) startY++; 
            
            while (_logicMap[startX, startY] != (int)MapType.None)
            {
                startX = Random.Range(0, roomToMazeData.map.width - 2); 
                startY = Random.Range(0, roomToMazeData.map.height - 2); 
                if (startX % 2 == 1) startX++;
                if (startY % 2 == 1) startY++;
            }

            return (startX, startY);
        }

        private bool IsValidCell(int x, int y)
        {
            return x >= 0 && x < roomToMazeData.map.width && y >= 0 && y < roomToMazeData.map.height;
        }

        #endregion
        
        #region Connect Map
        
        private void ConnectRooms()
        {
            BuildConnectablePosition(out List<Vector2Int> listPossiblePortals);

            foreach (var portal in listPossiblePortals)
            {
                foreach (var room in _listRooms)
                {
                    if (!room.IsValidPortal(portal.x, portal.y))
                    {
                        continue;
                    }
                    
                    if (room.IsHadPortalSameSide(portal.x, portal.y))
                    {
                        for (var iter = 0; iter < room.listPortals.Count; iter++)
                        {
                            if (portal.x == room.listPortals[iter].x || portal.y == room.listPortals[iter].y)
                            {
                                if (Random.Range(0, 100) < roomToMazeData.chanceToChangePortal)
                                {
                                    _logicMap[room.listPortals[iter].x, room.listPortals[iter].y] = (int)MapType.Dot;
                                    room.listPortals[iter] = portal;
                                    _logicMap[portal.x, portal.y] = (int)MapType.Maze;
                                }
                                break;
                            }
                        }
                        
                        continue;
                    }
                    
                    if (room.isConnectted)
                    {
                        if (Random.Range(0, 100) < roomToMazeData.imperfectRate)
                        {
                            room.listPortals.Add(portal);
                            _logicMap[portal.x, portal.y] = (int)MapType.Maze;
                        }
                    }
                    else
                    {
                        room.listPortals.Add(portal);
                        room.isConnectted = true;
                        _logicMap[portal.x, portal.y] = (int)MapType.Maze;
                    }
                }
            }
        }

        private void BuildConnectablePosition(out List<Vector2Int> listPossiblePortals)
        {
            listPossiblePortals = new List<Vector2Int>();
            for (var i = 1; i < roomToMazeData.map.width - 1; i++)
            {
                for (var j = 1; j < roomToMazeData.map.height - 1; j++)
                {
                    if (IsValidConnectPosition(i, j))
                    {
                        _logicMap[i, j] = (int)MapType.Dot;
                        listPossiblePortals.Add(new Vector2Int(i, j));
                    }
                }
            }
        }

        private bool IsValidConnectPosition(int x, int y)
        {
            if ((_logicMap[x, y + 1] == (int)MapType.Maze &&
                 _logicMap[x, y - 1] == (int)MapType.Floor) || 
                (_logicMap[x, y - 1] == (int)MapType.Maze &&
                 _logicMap[x, y + 1] == (int)MapType.Floor) || 
                (_logicMap[x + 1, y] == (int)MapType.Maze &&
                 _logicMap[x - 1, y] == (int)MapType.Floor) ||
                (_logicMap[x - 1, y] == (int)MapType.Maze &&
                 _logicMap[x + 1, y ] == (int)MapType.Floor))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Remove Dead End

        private void RemoveDeadEnds()
        {
            for (int i = 0; i < roomToMazeData.map.width; i++)
            {
                for (int j = 0; j < roomToMazeData.map.height; j++)
                {
                    if (IsDeadEnd(i, j))
                    {
                        _deadEnds.Enqueue(new Vector2Int(i, j));
                        continue;
                    }

                    if (IsTooActivePosition(i, j) && Random.Range(0, 100) < roomToMazeData.removeMazeRate)
                    {
                        _logicMap[i, j] = (int)MapType.None;
                        var direction = new [] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};
                        foreach (var dir in direction)
                        {
                            if (IsDeadEnd(i + dir.x, j + dir.y))
                            {
                                _deadEnds.Enqueue(new Vector2Int(i + dir.x, j + dir.y));
                            }
                        }
                    }
                }
            }
            
            
            
            var queueRemoveDeadEnds = new Queue<Vector2Int>();
            while (_deadEnds.Count > 0)
            {
                var position = _deadEnds.Dequeue();
                queueRemoveDeadEnds.Enqueue(position);
                _logicMap[position.x, position.y] = (int)MapType.None;
                var nextPossibleDeadEnd = position + GetDirDeadEnd(position.x, position.y);
                if(GetDirDeadEnd(position.x, position.y) == Vector2Int.zero) Debug.Log("-----");
                if (IsDeadEnd(nextPossibleDeadEnd.x, nextPossibleDeadEnd.y))
                {
                    _deadEnds.Enqueue(nextPossibleDeadEnd);
                }
            }
        }

        private bool IsDeadEnd(int i, int j)
        {
            var direction = new [] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};

            var deadDirection = 0;
            foreach (var dir in direction)
            {
                if (IsValidCell(i + dir.x, j + dir.y))
                {
                    if (_logicMap[i + dir.x, j + dir.y] == (int)MapType.None || _logicMap[i + dir.x, j + dir.y] == (int)MapType.Dot)
                    {
                        deadDirection++;
                    }
                }
                else deadDirection++;
            }

            return deadDirection == 3;
        }

        private bool IsTooActivePosition(int i, int j)
        {
            var direction = new [] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};

            var deadDirection = 0;
            foreach (var dir in direction)
            {
                if (IsValidCell(i + dir.x, j + dir.y))
                {
                    if (_logicMap[i + dir.x, j + dir.y] == (int)MapType.Maze)
                    {
                        deadDirection++;
                    }
                }
                else deadDirection++;
            }

            return deadDirection == 3;
        }

        private Vector2Int GetDirDeadEnd(int i, int j)
        {
            var direction = new [] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};
            var dirReturn = Vector2Int.zero;

            foreach (var dir in direction)
            {
                if(IsValidCell(i + dir.x, j + dir.y) == false) continue;
                if (_logicMap[i + dir.x, j + dir.y] == (int)MapType.None || _logicMap[i + dir.x, j + dir.y] == (int)MapType.Dot) continue;
                dirReturn = dir;
                break;
            }

            return dirReturn;
        }

        #endregion

    }

    internal enum MapType
    {
        None, Floor, Wall, Maze, Dot
    }
    
}
