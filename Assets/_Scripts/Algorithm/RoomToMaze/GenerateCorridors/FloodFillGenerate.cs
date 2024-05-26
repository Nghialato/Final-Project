using System.Collections.Generic;
using _Scripts.GameEts;
using UnityEngine;

namespace _Scripts.Algorithm.GenerateCorridors
{
    public class FloodFillGenerate : GeneratorCorridorsAbstract
    {
        public override void Generate(RoomToMazeData roomToMazeData, ref int[,] logicMap, out Queue<Vector2Int> _mazeQueue)
        {
            var startPos = roomToMazeData.PickStartPos(logicMap);
            _mazeQueue = new Queue<Vector2Int>();

            logicMap[startPos.Item1, startPos.Item2] = (int)MapType.Maze;

            var queue = new Queue<(Vector2Int, Vector2Int)>();
            queue.Enqueue((new Vector2Int(startPos.Item1, startPos.Item2), Vector2Int.up));
            _mazeQueue.Enqueue(new Vector2Int(startPos.Item1, startPos.Item2));

            var directions = new [] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                var direction = current.Item2;
                
                if(Random.Range(0, 100) < roomToMazeData.percentChangeDirection) 
                {
                    // Change Direction
                    directions.Shuffle();
                    direction = direction == directions[0] ? directions[1] : directions[0];
                }
                var neighbor = current.Item1 + direction * 2;

                if (roomToMazeData.IsValidCell(neighbor.x, neighbor.y) && logicMap[neighbor.x, neighbor.y] == (int)MapType.None)
                {
                    logicMap[neighbor.x, neighbor.y] = (int)MapType.Maze;
                    logicMap[neighbor.x - direction.x, neighbor.y - direction.y] = (int)MapType.Maze;
                    queue.Enqueue((neighbor, direction));
                    _mazeQueue.Enqueue(new Vector2Int(neighbor.x - direction.x, neighbor.y - direction.y));
                    _mazeQueue.Enqueue(new Vector2Int(neighbor.x, neighbor.y));
                }
                else
                {
                    foreach (var dir in directions)
                    {
                        var nei = current.Item1 + dir * 2;

                        if (roomToMazeData.IsValidCell(nei.x, nei.y) && logicMap[nei.x, nei.y] == (int)MapType.None)
                        {
                            logicMap[nei.x, nei.y] = (int)MapType.Maze;
                            logicMap[nei.x - dir.x, nei.y - dir.y] = (int)MapType.Maze;
                            queue.Enqueue((nei, dir));
                            _mazeQueue.Enqueue(new Vector2Int(nei.x - dir.x, nei.y - dir.y));
                            _mazeQueue.Enqueue(new Vector2Int(nei.x, nei.y));
                        }
                    }
                }
            }
        }
    }
}