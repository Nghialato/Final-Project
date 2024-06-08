using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Algorithm.GenerateCorridors
{
    [Serializable]
    public abstract class GeneratorCorridorsAbstract : MonoBehaviour, IGenerateCorridors
    {
        public abstract void Generate(MapData mapData, ref int[,] logicMap, out Queue<Vector2Int> mazeQueue);
    }
}