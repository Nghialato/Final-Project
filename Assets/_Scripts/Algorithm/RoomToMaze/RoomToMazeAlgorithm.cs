using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.Algorithm.ConnectRoomsAndCorridors;
using _Scripts.Algorithm.GenerateCorridors;
using _Scripts.Algorithm.GenerateRoom;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Algorithm
{
    public class RoomToMazeAlgorithm : AbstractDungeonGenerator
    {
        public RoomToMazeData roomToMazeData;

        public GenerateRoomAbstract Room;
        public GeneratorCorridorsAbstract Corridors;
        public ConnectRoomsToCorridorsAbstract ConnectRoomsAndCorridors;
        public RemoveDeadEnds RemoveDeadEnd; 

        private int[,] _logicMap;

        private Queue<Vector2Int> _mazeQueue = new();

        [SerializeField] private List<RoomData> _listRooms = new();
        private Queue<Vector2Int> _deadEnds;
        
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
            
            Room.Generate(roomToMazeData, ref _logicMap, out _listRooms);
            
            Corridors.Generate(roomToMazeData, ref _logicMap, out _mazeQueue);
            
            ConnectRoomsAndCorridors.Connect(roomToMazeData, ref _logicMap, ref _listRooms);
            
            RemoveDeadEnds(out var queueRemoveDeadEnds);
            
            foreach (var room in _listRooms)
            {
                for (int i = 0; i < room.width; i++)
                {
                    _logicMap[room.roomPos.x + i, room.roomPos.y] = (int)MapType.None;
                    if (_logicMap[room.roomPos.x + i, room.roomPos.y - 1] == (int)MapType.Maze)
                    {
                        _logicMap[room.roomPos.x + i, room.roomPos.y] = (int)MapType.Maze;
                    }
                    
                    _logicMap[room.roomPos.x + i, room.roomPos.y + room.height - 1] = (int)MapType.None;
                    if (_logicMap[room.roomPos.x + i, room.roomPos.y + room.height] == (int)MapType.Maze)
                    {
                        _logicMap[room.roomPos.x + i, room.roomPos.y + room.height - 1] = (int)MapType.Maze;
                    }
                }
                
                for (int i = 0; i < room.height; i++)
                {
                    _logicMap[room.roomPos.x, room.roomPos.y + i] = (int)MapType.None;
                    if (_logicMap[room.roomPos.x - 1, room.roomPos.y + i] == (int)MapType.Maze)
                    {
                        _logicMap[room.roomPos.x, room.roomPos.y + i] = (int)MapType.Maze;
                    }
                    
                    _logicMap[room.roomPos.x + room.width - 1, room.roomPos.y + i] = (int)MapType.None;
                    if (_logicMap[room.roomPos.x + room.width, room.roomPos.y + i] == (int)MapType.Maze)
                    {
                        _logicMap[room.roomPos.x + room.width - 1, room.roomPos.y + i] = (int)MapType.Maze;
                    }
                }
            }
            
            RemoveDeadEnds(out var queueRemoveDead);

            foreach (var room in _listRooms)
            {
                if (room.isConnectted == false)
                {
                    for (var y = room.roomPos.y; y < room.roomPos.y + room.height; y++)
                    {
                        for (var x = room.roomPos.x; x < room.roomPos.x + room.width; x++)
                        {
                            _logicMap[x, y] = (int)MapType.None;
                        }
                    }
                }
            }
            
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
            floor.Clear();
            
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
                        floor.Add(new Vector2Int(i, j));
                    }
                }
            }
            
            WallGenerator.CreateWalls(floor, tilemapVisualizer);
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

        #region Remove Dead End

        private void RemoveDeadEnds(out Queue<Vector2Int> queueRemoveDeadEnds)
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
            
            
            
            queueRemoveDeadEnds = new Queue<Vector2Int>();
            while (_deadEnds.Count > 0)
            {
                var position = _deadEnds.Dequeue();
                queueRemoveDeadEnds.Enqueue(position);
                _logicMap[position.x, position.y] = (int)MapType.None;
                var nextPossibleDeadEnd = position + GetDirDeadEnd(position.x, position.y);
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
                if (roomToMazeData.IsValidCell(i + dir.x, j + dir.y))
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
                if (roomToMazeData.IsValidCell(i + dir.x, j + dir.y))
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
                if(roomToMazeData.IsValidCell(i + dir.x, j + dir.y) == false) continue;
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
