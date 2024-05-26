using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Algorithm.GenerateRoom
{
    [Serializable]
    public abstract class GenerateRoomAbstract : MonoBehaviour, IGenerateRoom
    {
         public abstract void Generate(RoomToMazeData roomToMazeData, ref int[,] logicMap, out List<RoomData> listRooms);
    }
}