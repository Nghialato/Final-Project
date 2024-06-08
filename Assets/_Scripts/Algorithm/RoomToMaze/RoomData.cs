using System;
using System.Collections.Generic;
using UnityEngine;

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
            var validPos = (i >= roomPos.x + 2 && i <= roomPos.x + width - 2 && (j == roomPos.y || j == roomPos.y + height)) ||
                           (j >= roomPos.y + 2 && j <= roomPos.y + height - 2 && (i == roomPos.x || i == roomPos.x + width));
                
            return validPos;
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
    }
}