using System;
using System.Collections.Generic;

namespace _Scripts.Algorithm.GenerateRoom
{
    public interface IGenerateRoom
    {
        public void Generate(MapData mapData, ref int[,] logicMap, out List<RoomData> listRooms);
    }
}