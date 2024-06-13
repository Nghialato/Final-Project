using _Scripts.Algorithm;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class SelectCorridorGen : MonoBehaviour
{
    private TMP_Dropdown selectCorridorsGen; 
    [FormerlySerializedAs("roomToMazeAlgorithm")] [SerializeField] private DungeonGenerator dungeonGenerator;
    public GameObject mazeData;

    private void Awake()
    {
        selectCorridorsGen = GetComponent<TMP_Dropdown>();
    }

    private void Start()
    {
        selectCorridorsGen.onValueChanged.AddListener(indexAlgorithm =>
        {
            dungeonGenerator.SelectCorridorsGenerateAlgorithm(indexAlgorithm);
            mazeData.SetActive(indexAlgorithm == 1);
        });
    }
}
