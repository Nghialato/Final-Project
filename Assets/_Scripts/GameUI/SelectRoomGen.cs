using System.Collections.Generic;
using _Scripts.Algorithm;
using TMPro;
using UnityEngine;

public class SelectRoomGen : MonoBehaviour
{
    private TMP_Dropdown selectRoomGen;
    [SerializeField] private DungeonGenerator dungeonGenerator;
    public List<GameObject> listData;

    private void Awake()
    {
        selectRoomGen = GetComponent<TMP_Dropdown>();
    }

    private void Start()
    {
        selectRoomGen.onValueChanged.AddListener(indexAlgorithm =>
        {
            dungeonGenerator.SelectRoomGenerateAlgorithm(indexAlgorithm);
            for (int i = 0; i < listData.Count; i++)
            {
                listData[i].SetActive(i == indexAlgorithm);
            }
        });
    }
}
