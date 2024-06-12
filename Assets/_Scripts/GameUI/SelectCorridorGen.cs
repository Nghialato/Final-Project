using _Scripts.Algorithm;
using TMPro;
using UnityEngine;

public class SelectCorridorGen : MonoBehaviour
{
    private TMP_Dropdown selectCorridorsGen; 
    [SerializeField] private RoomToMazeAlgorithm roomToMazeAlgorithm;
    public GameObject mazeData;

    private void Awake()
    {
        selectCorridorsGen = GetComponent<TMP_Dropdown>();
    }

    private void Start()
    {
        selectCorridorsGen.onValueChanged.AddListener(indexAlgorithm =>
        {
            roomToMazeAlgorithm.SelectCorridorsGenerateAlgorithm(indexAlgorithm);
            mazeData.SetActive(indexAlgorithm == 1);
        });
    }
}
