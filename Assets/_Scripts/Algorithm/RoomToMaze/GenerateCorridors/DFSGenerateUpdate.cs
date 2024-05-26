using System.Collections.Generic;
using _Scripts.GameEts;
using UnityEngine;

namespace _Scripts.Algorithm.GenerateCorridors
{
    public class DFSGenerateUpdate : GeneratorCorridorsAbstract
    {
        public int stepCheck;
        public override void Generate(RoomToMazeData roomToMazeData, ref int[,] logicMap, out Queue<Vector2Int> _mazeQueue)
        {
            _mazeQueue = new Queue<Vector2Int>();
            var percentChangeDirection = 40;

            var startPos = roomToMazeData.PickStartPos(logicMap);

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
                        var nextStep = position.Item1 + position.Item2 * stepCheck;
                        if (roomToMazeData.IsValidCell(nextStep.x, nextStep.y) && logicMap[nextStep.x, nextStep.y] == (int)MapType.None)
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
                    var check = current.Item1 + dir * stepCheck;

                    if (roomToMazeData.IsValidCell(check.x, check.y) && logicMap[check.x, check.y] == (int)MapType.None)
                    {
                        possibleMoveFromStartPos.Enqueue((current.Item1, dir));
                    }
                }
                
                if(Random.Range(0, 100) < percentChangeDirection) 
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
                
                var haveDirectionMove = false;
                foreach (var dir in directions)
                {
                    var neighbor = current.Item1 + dir * stepCheck;

                    if (roomToMazeData.IsValidCell(neighbor.x, neighbor.y) && logicMap[neighbor.x, neighbor.y] == (int)MapType.None)
                    {
                        if (haveDirectionMove == false)
                        {
                            queue.Enqueue((neighbor, dir));
                            for (int i = stepCheck - 1; i >= 0; i--)
                            {
                                logicMap[neighbor.x - dir.x * i, neighbor.y - dir.y * i] = (int)MapType.Maze;
                                _mazeQueue.Enqueue(new Vector2Int(neighbor.x - dir.x * i, neighbor.y - dir.y * i));
                            }
                            haveDirectionMove = true;
                            continue;
                        }
                        possibleMoveFromStartPos.Enqueue((current.Item1, dir));
                    }
                }
            }
        }
    }
}