using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Algorithm.GenerateRoom
{
    public class RoomsBSPGenerate : GenerateRoomAbstract
    {
        private Queue<AreaInMap> _listAreas = new ();
        public override void Generate(MapData mapData, ref int[,] logicMap, out List<RoomData> listRooms)
        {
            SeparateMapArea(mapData);
            PlaceRoomsToArea(out listRooms, mapData.numRoomsRequired);

            foreach (var room in listRooms)
            {
                for (var y = room.roomPos.y; y < room.roomPos.y + room.height; y++)
                {
                    for (var x = room.roomPos.x; x < room.roomPos.x + room.width; x++)
                    {
                        logicMap[x, y] = (int)MapType.Floor;
                    }
                }
            }
        }

        private class AreaInMap
        {
            public int posX;
            public int posY;
            public int width;
            public int height;
            public bool isUsed;

            public AreaInMap(int posX, int posY, int width, int height)
            {
                this.posX = posX;
                this.posY = posY;
                this.width = width;
                this.height = height;
            }

            public void RandomSeparate(out AreaInMap area1, out AreaInMap area2)
            {
                var rate = Random.Range(0, 100);

                if (width > height * 2)
                {
                    rate = 25;
                } else if (height > width * 2)
                {
                    rate = 75;
                }
                
                // Random Separate by Horizontal or Vertical
                if (rate > 50)
                {
                    // Separate by horizontal
                    area1 = new AreaInMap(posX, posY, width, height / 2);
                    area2 = new AreaInMap(posX, posY + height / 2, width, height / 2);
                }
                else
                {
                    // Separate by vertical
                    area1 = new AreaInMap(posX, posY, width / 2, height);
                    area2 = new AreaInMap(posX + width / 2, posY, width / 2, height);
                }
            }
        }

        private void SeparateMapArea(in MapData mapData)
        {
            var initArea = new AreaInMap(2, 2, mapData.mapSize.width - 2, mapData.mapSize.height - 2);
            _listAreas.Clear();
            _listAreas.Enqueue(initArea);
            var numRoomsRequired = mapData.numRoomsRequired;
            var timesSeparate = 2;
            while (numRoomsRequired / 2 != 0)
            {
                timesSeparate *= 2;
                numRoomsRequired /= 2;
            }
            
            while (timesSeparate-- != 1)
            {
                var area = _listAreas.Dequeue();
                area.RandomSeparate(out var area1, out var area2);
                _listAreas.Enqueue(area1);
                _listAreas.Enqueue(area2);
            }
            
        }

        private void PlaceRoomsToArea(out List<RoomData> listRooms, int numRoomsRequired)
        {
            listRooms = new List<RoomData>();
            var listAreas = _listAreas.ToArray();
            var countArea = listAreas.Length;

            while (numRoomsRequired-- != 0)
            {
                var idRoom = Random.Range(0, countArea);
                while (listAreas[idRoom].isUsed)
                {
                    idRoom = Random.Range(0, countArea);
                }
                var area = listAreas[idRoom];
                
                var roomWidth = Random.Range(area.width / 2, area.width);
                var roomHeight = Random.Range(area.height / 2, area.height);

                if (area.width > area.height * 2)
                {
                    roomWidth = roomHeight + Random.Range(-3, 3);
                } else if (area.height > area.width * 2)
                {
                    roomHeight = roomWidth + Random.Range(-3, 3);
                }

                roomWidth -= 1 - roomWidth % 2;
                roomHeight -= 1 - roomHeight % 2;
                
                var xPos = Random.Range(area.posX + 2, area.posX + area.width - roomWidth);
                var yPos = Random.Range(area.posY + 2, area.posY + area.height - roomHeight);

                xPos -= xPos % 2;
                yPos -= yPos % 2;
                
                var roomNew = new RoomData(numRoomsRequired, new Vector2Int(xPos, yPos),
                    roomWidth, roomHeight);
                listAreas[idRoom].isUsed = true;
                listRooms.Add(roomNew);
            }
            
        }
    }
}