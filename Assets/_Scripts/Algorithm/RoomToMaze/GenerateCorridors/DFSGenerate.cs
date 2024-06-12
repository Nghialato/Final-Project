using System.Collections.Generic;
using _Scripts.Algorithm.Data;
using _Scripts.GameEts;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace _Scripts.Algorithm.GenerateCorridors
{
    public class DFSGenerate : GeneratorCorridorsAbstract
    {
        [SerializeField] private FloodFillGenerateData floodFillGenerateData;
        public override void Generate(MapData mapData, ref int[,] logicMap, in List<RoomData> listRoom, out Queue<Vector2Int> _mazeQueue)
        {
            _mazeQueue = new Queue<Vector2Int>();
            var startPos = mapData.PickStartPos(logicMap);

            logicMap[startPos.Item1, startPos.Item2] = (int)MapType.Maze;

            var queuePosition = new Queue<(Vector2Int, Vector2Int)>();
            queuePosition.Enqueue((new Vector2Int(startPos.Item1, startPos.Item2), Vector2Int.up));
            _mazeQueue.Enqueue(new Vector2Int(startPos.Item1, startPos.Item2));

            var directions = new List<Vector2Int> { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};
            
            while (queuePosition.Count > 0)
            {
                // Get position and direction
                var (position, direction) = queuePosition.Dequeue();

                // Calculate Direction
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
                    // Keep Direction
                    directions.Remove(direction);
                    directions.Add(direction);
                    directions.Reverse();
                }
                
                // Check Next Point
                foreach (var dir in directions)
                {
                    var nextPoint = position + dir * 2;

                    if (mapData.IsValidCell(nextPoint.x, nextPoint.y) && logicMap[nextPoint.x, nextPoint.y] == (int)MapType.None)
                    {
                        logicMap[nextPoint.x, nextPoint.y] = (int)MapType.Maze;
                        logicMap[nextPoint.x - dir.x, nextPoint.y - dir.y] = (int)MapType.Maze;
                        queuePosition.Enqueue((nextPoint, dir));
                        _mazeQueue.Enqueue(new Vector2Int(nextPoint.x - dir.x, nextPoint.y - dir.y));
                        _mazeQueue.Enqueue(new Vector2Int(nextPoint.x, nextPoint.y));
                        break;
                    }
                }
            }
        }
    }
}