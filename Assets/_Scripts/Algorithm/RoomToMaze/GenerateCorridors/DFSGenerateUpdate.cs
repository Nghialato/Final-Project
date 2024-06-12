using System.Collections.Generic;
using _Scripts.Algorithm.Data;
using _Scripts.GameEts;
using UnityEngine;

namespace _Scripts.Algorithm.GenerateCorridors
{
    public class DFSGenerateUpdate : GeneratorCorridorsAbstract
    {
        [SerializeField] private FloodFillGenerateData floodFillGenerateData;

        public override void Generate(MapData mapData, ref int[,] logicMap, in List<RoomData> listRoom, out Queue<Vector2Int> _mazeQueue)
        {
            _mazeQueue = new Queue<Vector2Int>();

            var startPos = mapData.PickStartPos(logicMap);

            logicMap[startPos.Item1, startPos.Item2] = (int)MapType.Maze;

            var queue = new Queue<(Vector2Int, Vector2Int)>();
            queue.Enqueue((new Vector2Int(startPos.Item1, startPos.Item2), Vector2Int.up));
            _mazeQueue.Enqueue(new Vector2Int(startPos.Item1, startPos.Item2));

            var directions = new List<Vector2Int> { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};

            var possibleMoveFromStartPos = new Queue<(Vector2Int, Vector2Int)>();

            while (queue.Count > 0 || possibleMoveFromStartPos.Count > 0)
            {
                if (queue.Count == 0)
                {
                    while (possibleMoveFromStartPos.Count > 0)
                    {
                        var position = possibleMoveFromStartPos.Dequeue();
                        var nextStep = position.Item1 + position.Item2 * floodFillGenerateData.stepMove;
                        if (mapData.IsValidCell(nextStep.x, nextStep.y) && logicMap[nextStep.x, nextStep.y] == (int)MapType.None)
                        {
                            queue.Enqueue((position.Item1, position.Item2));
                            while (possibleMoveFromStartPos.Peek().Item1 == position.Item1)
                            {
                                possibleMoveFromStartPos.Dequeue();
                            }
                            break;
                        }
                    }

                    if (possibleMoveFromStartPos.Count == 0) break;
                }
                var current = queue.Dequeue();

                var direction = current.Item2;
                
                // Add Possible Move
                
                foreach (var dir in directions)
                {
                    var check = current.Item1 + dir * floodFillGenerateData.stepMove;

                    if (mapData.IsValidCell(check.x, check.y) && logicMap[check.x, check.y] == (int)MapType.None)
                    {
                        possibleMoveFromStartPos.Enqueue((current.Item1, dir));
                    }
                }
                
                if(Random.Range(0, 100) < floodFillGenerateData.percentChangeDirection) 
                {
                    // Change Direction
                    directions.Shuffle();
                    if (directions[0] == direction)
                    {
                        (directions[0], directions[1]) = (directions[1], directions[0]);
                    }
                } else
                {
                    // Move to end Point
                    directions.Remove(direction);
                    directions.Add(direction);
                    directions.Reverse();
                }
                
                var foundDirectionMove = false;
                foreach (var dir in directions)
                {
                    var neighbor = current.Item1 + dir * floodFillGenerateData.stepMove;

                    if (mapData.IsValidCell(neighbor.x, neighbor.y) && logicMap[neighbor.x, neighbor.y] == (int)MapType.None)
                    {
                        if (foundDirectionMove == false)
                        {
                            queue.Enqueue((neighbor, dir));
                            for (int i = floodFillGenerateData.stepMove - 1; i >= 0; i--)
                            {
                                logicMap[neighbor.x - dir.x * i, neighbor.y - dir.y * i] = (int)MapType.Maze;
                                _mazeQueue.Enqueue(new Vector2Int(neighbor.x - dir.x * i, neighbor.y - dir.y * i));
                            }
                            foundDirectionMove = true;
                            continue;
                        }
                        possibleMoveFromStartPos.Enqueue((current.Item1, dir));
                    }
                }
            }
        }
    }
}