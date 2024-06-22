using _Scripts.Algorithm.Data;
using TMPro;
using UnityEngine;

public class BSPDataUI : MonoBehaviour
{
    [SerializeField] private RoomBSPGenerateData bspRoomData;
    [SerializeField] private TMP_InputField percentSplitHorizontal;

    private void Awake()
    {
        percentSplitHorizontal.onEndEdit.AddListener(num =>
        {
            bspRoomData.percentSplitHorizontal = int.Parse(num);
        });

        percentSplitHorizontal.text = bspRoomData.percentSplitHorizontal.ToString();
        percentSplitHorizontal.text = bspRoomData.percentSplitHorizontal.ToString();

    }
}
