using System;
using System.Collections.Generic;
using _Scripts.GameEts;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Algorithm.GenerateCorridors
{
    public class NearestGenerate : GeneratorCorridorsAbstract
    {
        public override void Generate(MapData mapData, ref int[,] logicMap, in List<RoomData> listRoom, out Queue<Vector2Int> mazeQueue)
        {
            mazeQueue = new Queue<Vector2Int>();
            var cacheRoomList = new List<RoomData>(listRoom);
            
            // Get Random Room
            cacheRoomList.Shuffle();
            var curRoom = cacheRoomList[0];
            cacheRoomList.Remove(curRoom);
            curRoom.isConnected = true;

            while (cacheRoomList.Count > 0)
            {
                var curPos = curRoom.GetCenter();
                GetClosetRoom(curPos, cacheRoomList, out var roomCloset);
                
                #region Connect 2 Rooms

                var direction = roomCloset.GetCenter() - curPos;
                var lengthX = Math.Abs(direction.x);
                var lengthY = Math.Abs(direction.y);
                var xDirection = lengthX != 0 ? new Vector2Int(direction.x / lengthX, 0) : Vector2Int.zero;
                var yDirection = lengthY != 0 ? new Vector2Int(0, direction.y / lengthY) : Vector2Int.zero;

                while (lengthX > 0)
                {
                    curPos += xDirection;
                    lengthX--;
                    if (logicMap[curPos.x, curPos.y] == (int)MapType.None)
                    {
                        logicMap[curPos.x, curPos.y] = (int)MapType.Floor;
                    }
                }
                
                while (lengthY > 0)
                {
                    curPos += yDirection;
                    lengthY--;
                    if (logicMap[curPos.x, curPos.y] == (int)MapType.None)
                    {
                        logicMap[curPos.x, curPos.y] = (int)MapType.Floor;
                    }
                }

                #endregion
                
                curRoom = roomCloset;
                curRoom.isConnected = true;
                cacheRoomList.Remove(roomCloset);
            }
        }

        private void GetClosetRoom(Vector2Int roomCenter, in List<RoomData> roomData, out RoomData roomClosest)
        {
            roomClosest = roomData[0];
            var distance = Vector2Int.Distance(roomCenter, roomClosest.GetCenter());
            foreach (var room in roomData)
            {
                if (distance > Vector2Int.Distance(roomCenter, room.GetCenter()))
                {
                    roomClosest = room;
                }
            }
        }
    }
}