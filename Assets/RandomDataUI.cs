using _Scripts.Algorithm.Data;
using TMPro;
using UnityEngine;

public class RandomDataUI : MonoBehaviour
{
    [SerializeField] private RandomRoomData randomRoomData;
    [SerializeField] private TMP_InputField numRoomTriesInit;
    [SerializeField] private TMP_InputField percentFillMap;

    private void Awake()
    {
        numRoomTriesInit.onEndEdit.AddListener(num =>
        {
            randomRoomData.numRoomsTriesInit = int.Parse(num);
        });
        
        percentFillMap.onEndEdit.AddListener(percent =>
        {
            randomRoomData.percentFillMap = int.Parse(percent);
        });

        numRoomTriesInit.text = randomRoomData.numRoomsTriesInit.ToString();
        percentFillMap.text = randomRoomData.percentFillMap.ToString();

    }
}
