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
    public class DungeonGenerator : AbstractDungeonGenerator
    {
        public static DungeonGenerator Instance;
        public MapData mapData;

        public List<GenerateRoomAbstract> RoomGenerates;
        public List<GeneratorCorridorsAbstract> CorridorsGenerates;

        private GenerateRoomAbstract Room;
        private GeneratorCorridorsAbstract Corridors;
        public ConnectRoomsToCorridorsAbstract ConnectRoomsAndCorridors;

        private int[,] _logicMap;

        private Queue<Vector2Int> _mazeQueue = new();

        [SerializeField] private List<RoomData> _listRooms = new();
        private Queue<Vector2Int> _deadEnds;
        private int playerRoomID;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this.gameObject);
        }   

        private void Start()
        {
            SelectRoomGenerateAlgorithm(0);
            SelectCorridorsGenerateAlgorithm(0);
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
            
            _logicMap = new int[mapData.mapSize.width, mapData.mapSize.height];
            
            for (var i = 0; i < mapData.mapSize.width; i++)
            {
                for (var j = 0; j < mapData.mapSize.height; j++)
                {
                    _logicMap[i, j] = (int)MapType.None;
                }
            }
            
            Room.Generate(mapData, ref _logicMap, out _listRooms);
            
            Corridors.Generate(mapData, ref _logicMap, _listRooms, out _mazeQueue);
            
            ConnectRoomsAndCorridors.Connect(mapData, ref _logicMap, ref _listRooms);
            
            RemoveDeadEnds(out var queueRemoveDeadEnds);
            
            foreach (var room in _listRooms)
            {
                if (room.isConnected == false)
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
            
            for (int i = 0; i < mapData.mapSize.width; i++)
            {
                for (int j = 0; j < mapData.mapSize.height; j++)
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
            
            for (int i = 0; i < mapData.mapSize.width; i++)
            {
                for (int j = 0; j < mapData.mapSize.height; j++)
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
            if (mapData.mapSize.width % 2 == 0)
            {
                mapData.mapSize.width++;
            }
            
            if (mapData.mapSize.height % 2 == 0)
            {
                mapData.mapSize.height++;
            }
        }

        #region Remove Dead End

        private void RemoveDeadEnds(out Queue<Vector2Int> queueRemoveDeadEnds)
        {
            for (int i = 0; i < mapData.mapSize.width; i++)
            {
                for (int j = 0; j < mapData.mapSize.height; j++)
                {
                    if (IsDeadEnd(i, j))
                    {
                        _deadEnds.Enqueue(new Vector2Int(i, j));
                        continue;
                    }

                    // if (IsTooActivePosition(i, j) && Random.Range(0, 100) < 10)
                    // {
                    //     _logicMap[i, j] = (int)MapType.None;
                    //     var direction = new [] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};
                    //     foreach (var dir in direction)
                    //     {
                    //         if (IsDeadEnd(i + dir.x, j + dir.y))
                    //         {
                    //             _deadEnds.Enqueue(new Vector2Int(i + dir.x, j + dir.y));
                    //         }
                    //     }
                    // }
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
                if (mapData.IsValidCell(i + dir.x, j + dir.y))
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
                if (mapData.IsValidCell(i + dir.x, j + dir.y))
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
                if(mapData.IsValidCell(i + dir.x, j + dir.y) == false) continue;
                if (_logicMap[i + dir.x, j + dir.y] == (int)MapType.None || _logicMap[i + dir.x, j + dir.y] == (int)MapType.Dot) continue;
                dirReturn = dir;
                break;
            }

            return dirReturn;
        }

        #endregion

        public void SelectRoomGenerateAlgorithm(int indexAlgorithm)
        {
            Room = RoomGenerates[indexAlgorithm];
        }
        
        public void SelectCorridorsGenerateAlgorithm(int indexAlgorithm)
        {
            Corridors = CorridorsGenerates[indexAlgorithm];
        }

        public Vector2Int GetRandomPositionForPlayer()
        {
            var room = _listRooms[Random.Range(0, _listRooms.Count)];
            playerRoomID = room.roomId;
            return room.GetCenter();
        }

        public List<RoomData> GetListRoom() => _listRooms;

        public bool IsPlayerRoom(int id) => playerRoomID == id;

    }

    internal enum MapType
    {
        None, Floor, Wall, Maze, Dot
    }
}
