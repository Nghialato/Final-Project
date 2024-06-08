using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Algorithm.ConnectRoomsAndCorridors
{
    public abstract class ConnectRoomsToCorridorsAbstract : MonoBehaviour, IConnectRoomsAndCorridors
    {
        public abstract void Connect(MapData mapData, ref int[,] logicMap, ref List<RoomData> listRooms);
    }
}