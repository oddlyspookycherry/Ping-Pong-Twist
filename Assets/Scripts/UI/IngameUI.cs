using UnityEngine;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour
{
    [SerializeField]
    private Text player1;

    [SerializeField]
    private Text player2;

    public void UpdateScore(int first, int second)
    {
        player1.text = first.ToString();
        player2.text = second.ToString();
    }

    public void UpdateScoreFirst(int score)
    {
        player1.text = score.ToString();
    }

    public void UpdateScoreSecond(int score)
    {
        player2.text = score.ToString();
    }
}
