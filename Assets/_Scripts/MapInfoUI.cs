using System;
using _Scripts.Algorithm;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class MapInfoUI : MonoBehaviour
{
    [FormerlySerializedAs("roomToMazeAlgorithm")] [SerializeField] private DungeonGenerator dungeonGenerator;

    [SerializeField] private TMP_InputField mapWidth;
    [SerializeField] private TMP_InputField mapHeight;
    [SerializeField] private TMP_InputField numRooms;

    private void Awake()
    {
        mapWidth.onEndEdit.AddListener(width =>
        {
            dungeonGenerator.mapData.mapSize.width = int.Parse(width);
        });
        
        mapHeight.onEndEdit.AddListener(height =>
        {
            dungeonGenerator.mapData.mapSize.height = int.Parse(height);
        });
        
        numRooms.onEndEdit.AddListener(numRoom =>
        {
            dungeonGenerator.mapData.numRoomsRequired = int.Parse(numRoom);
        });

        mapWidth.text = dungeonGenerator.mapData.mapSize.width.ToString();
        mapHeight.text = dungeonGenerator.mapData.mapSize.height.ToString();
        numRooms.text = dungeonGenerator.mapData.numRoomsRequired.ToString();
    }
}
