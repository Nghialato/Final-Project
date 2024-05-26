using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Algorithm.ConnectRoomsAndCorridors
{
    public class ConnectRoomToMaze : ConnectRoomsToCorridorsAbstract
    {
        public override void Connect(RoomToMazeData roomToMazeData, ref int[,] logicMap, ref List<RoomData> listRooms)
        {
            BuildConnectablePosition(roomToMazeData, ref logicMap, out List<Vector2Int> listPossiblePortals);

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
                                if (Random.Range(0, 100) < roomToMazeData.chanceToChangePortal)
                                {
                                    logicMap[room.listPortals[iter].x, room.listPortals[iter].y] = (int)MapType.Dot;
                                    room.listPortals[iter] = portal;
                                    logicMap[portal.x, portal.y] = (int)MapType.Maze;
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
                            logicMap[portal.x, portal.y] = (int)MapType.Maze;
                        }
                    }
                    else
                    {
                        room.listPortals.Add(portal);
                        room.isConnectted = true;
                        logicMap[portal.x, portal.y] = (int)MapType.Maze;
                    }
                }
            }
        }

        private void BuildConnectablePosition(RoomToMazeData roomToMazeData, ref int[,] logicMap,out List<Vector2Int> listPossiblePortals)
        {
            listPossiblePortals = new List<Vector2Int>();
            for (var i = 1; i < roomToMazeData.map.width - 1; i++)
            {
                for (var j = 1; j < roomToMazeData.map.height - 1; j++)
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