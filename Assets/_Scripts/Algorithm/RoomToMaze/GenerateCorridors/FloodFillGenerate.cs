using System.Collections.Generic;
using _Scripts.Algorithm.Data;
using _Scripts.GameEts;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Algorithm.GenerateCorridors
{
    public class FloodFillGenerate : GeneratorCorridorsAbstract
    {
        [SerializeField] private FloodFillGenerateData floodFillGenerateData;
        public override void Generate(MapData mapData, ref int[,] logicMap, in List<RoomData> listRoom, out Queue<Vector2Int> mazeQueue)
        {
            var startPos = mapData.PickStartPos(logicMap);
            mazeQueue = new Queue<Vector2Int>();

            logicMap[startPos.Item1, startPos.Item2] = (int)MapType.Maze;

            var queue = new Queue<(Vector2Int, Vector2Int)>();
            queue.Enqueue((new Vector2Int(startPos.Item1, startPos.Item2), Vector2Int.up));
            mazeQueue.Enqueue(new Vector2Int(startPos.Item1, startPos.Item2));

            var directions = new List<Vector2Int> { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                var direction = current.Item2;
                
                if(Random.Range(0, 100) < floodFillGenerateData.percentChangeDirection) 
                {
                    // Change Direction
                    directions.Shuffle();
                    direction = direction == directions[0] ? directions[1] : directions[0];
                }
                else
                {
                    // Move to end Point
                    directions.Remove(direction);
                    directions.Add(direction);
                }
                
                foreach (var dir in directions)
                {
                    var nei = current.Item1 + dir * 2;

                    if (mapData.IsValidCell(nei.x, nei.y) && logicMap[nei.x, nei.y] == (int)MapType.None)
                    {
                        logicMap[nei.x, nei.y] = (int)MapType.Maze;
                        logicMap[nei.x - dir.x, nei.y - dir.y] = (int)MapType.Maze;
                        queue.Enqueue((nei, dir));
                        mazeQueue.Enqueue(new Vector2Int(nei.x - dir.x, nei.y - dir.y));
                        mazeQueue.Enqueue(new Vector2Int(nei.x, nei.y));
                    }
                }
            }
        }
    }
}