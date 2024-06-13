using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Algorithm
{
    [Serializable]
    public class RoomData
    {
        public int roomId;
        public Vector2Int roomPos;
        public int width;
        public int height;
        public bool isConnected = false;
        public List<Vector2Int> listPortals = new();

    public RoomData(int roomId, Vector2Int roomPos, int width, int height)
        {
            this.roomId = roomId;
            this.roomPos = roomPos;
            this.width = width;
            this.height = height;
            listPortals.Clear();
        }
    }

    public static class RoomDataEts
    {
        public static bool IsValidPortal(this RoomData roomData, int i, int j)
        {
            var roomPos = roomData.roomPos;
            var width = roomData.width;
            var height = roomData.height;
            return (i >= roomPos.x + 2 && i <= roomPos.x + width - 2 && (j == roomPos.y || j == roomPos.y + height)) ||
                           (j >= roomPos.y + 2 && j <= roomPos.y + height - 2 && (i == roomPos.x || i == roomPos.x + width));
        }

        public static bool IsInside(this RoomData roomData, int i, int j)
        {
            var roomPos = roomData.roomPos;
            var width = roomData.width;
            var height = roomData.height;
            return i >= roomPos.x && i < roomPos.x + width && j >= roomPos.y && j < roomPos.y + height;
        }

        public static bool IsHadPortalSameSide(this RoomData roomData, int i, int j)
        {
            var listPortals = roomData.listPortals;
            foreach (var portal in listPortals)
            {
                if (portal.x == i || portal.y == j)
                {
                    return true;
                }
            }

            return false;
        }

        public static Vector2Int GetCenter(this RoomData roomData)
        {
            return roomData.roomPos + new Vector2Int(roomData.width / 2, roomData.height / 2);
        }

        public static Vector2Int GetRandomPointInside(this RoomData roomData)
        {
            return roomData.roomPos + new Vector2Int(Random.Range(0, roomData.width - 1), Random.Range(0, roomData.height - 1));
        }
    }
}