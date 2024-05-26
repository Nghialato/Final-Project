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
}