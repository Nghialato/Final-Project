using System.Collections.Generic;
using _Scripts.Algorithm;
using TMPro;
using UnityEngine;

public class SelectRoomGen : MonoBehaviour
{
    private TMP_Dropdown selectRoomGen; 
    [SerializeField] private RoomToMazeAlgorithm roomToMazeAlgorithm;
    public List<GameObject> listData;

    private void Awake()
    {
        selectRoomGen = GetComponent<TMP_Dropdown>();
    }

    private void Start()
    {
        selectRoomGen.onValueChanged.AddListener(indexAlgorithm =>
        {
            roomToMazeAlgorithm.SelectRoomGenerateAlgorithm(indexAlgorithm);
            for (int i = 0; i < listData.Count; i++)
            {
                listData[i].SetActive(i == indexAlgorithm);
            }
        });
    }
}
