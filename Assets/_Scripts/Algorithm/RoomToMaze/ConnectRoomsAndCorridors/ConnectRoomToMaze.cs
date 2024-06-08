using System.Collections.Generic;
using _Scripts.Algorithm.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Algorithm.ConnectRoomsAndCorridors
{
    public class ConnectRoomToMaze : ConnectRoomsToCorridorsAbstract
    {
        [SerializeField] private ConnectRoomToMazeData connectRoomToMazeData;
        public override void Connect(MapData mapData, ref int[,] logicMap, ref List<RoomData> listRooms)
        {
            BuildConnectablePosition(mapData, ref logicMap, out List<Vector2Int> listPossiblePortals);

            foreach (var portal in listPossiblePortals)
            {
                foreach (var room in listRooms)
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
                                if (Random.Range(0, 100) < connectRoomToMazeData.chanceToChangePortal)
                                {
                                    logicMap[room.listPortals[iter].x, room.listPortals[iter].y] = (int)MapType.None;
                                    room.listPortals[iter] = portal;
                                    logicMap[portal.x, portal.y] = (int)MapType.Maze;
                                }
                                break;
                            }
                        }
                        
                        continue;
                    }
                    
                    if (room.isConnected)
                    {
                        if (Random.Range(0, 100) < connectRoomToMazeData.imperfectRate)
                        {
                            room.listPortals.Add(portal);
                            logicMap[portal.x, portal.y] = (int)MapType.Maze;
                        }
                    }
                    else
                    {
                        room.listPortals.Add(portal);
                        room.isConnected = true;
                        logicMap[portal.x, portal.y] = (int)MapType.Maze;
                        break;
                    }
                }
            }
        }

        private void BuildConnectablePosition(MapData mapData, ref int[,] logicMap,out List<Vector2Int> listPossiblePortals)
        {
            listPossiblePortals = new List<Vector2Int>();
            for (var i = 1; i < mapData.mapSize.width - 1; i++)
            {
                for (var j = 1; j < mapData.mapSize.height - 1; j++)
                {
                    if (IsValidConnectPosition(i, j, logicMap))
                    {
                        logicMap[i, j] = (int)MapType.Dot;
                        listPossiblePortals.Add(new Vector2Int(i, j));
                    }
                }
            }
        }

        private bool IsValidConnectPosition(int x, int y, in int[,] logicMap)
        {
            if ((logicMap[x, y + 1] == (int)MapType.Maze &&
                 logicMap[x, y - 1] == (int)MapType.Floor) || 
                (logicMap[x, y - 1] == (int)MapType.Maze &&
                 logicMap[x, y + 1] == (int)MapType.Floor) || 
                (logicMap[x + 1, y] == (int)MapType.Maze &&
                 logicMap[x - 1, y] == (int)MapType.Floor) ||
                (logicMap[x - 1, y] == (int)MapType.Maze &&
                 logicMap[x + 1, y ] == (int)MapType.Floor))
            {
                return true;
            }

            return false;
        }
    }
}