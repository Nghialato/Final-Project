using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Algorithm.GenerateCorridors
{
    public interface IGenerateCorridors
    {
        public void Generate(MapData mapData, ref int[,] logicMap, out Queue<Vector2Int> _mazeQueue);
    }
}