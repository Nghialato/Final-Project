using System.Collections.Generic;

namespace _Scripts.Algorithm.ConnectRoomsAndCorridors
{
    public interface IConnectRoomsAndCorridors
    {
        public void Connect(RoomToMazeData roomToMazeData, ref int[,] logicMap, ref List<RoomData> listRooms);
    }
}