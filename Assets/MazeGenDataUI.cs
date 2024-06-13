using _Scripts.Algorithm.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class MazeGenDataUI : MonoBehaviour
{
    [SerializeField] private FloodFillGenerateData mazeGenData;
    [SerializeField] private TMP_InputField stepMove;
    [SerializeField] private TMP_InputField percentChangeDirection;

    private void Awake()
    {
        stepMove.onEndEdit.AddListener(num =>
        {
            mazeGenData.stepMove = int.Parse(num);
        });
        
        percentChangeDirection.onEndEdit.AddListener(num =>
        {
            mazeGenData.percentChangeDirection = int.Parse(num);
        });

        stepMove.text = mazeGenData.stepMove.ToString();
        percentChangeDirection.text = mazeGenData.percentChangeDirection.ToString();

    }
}
