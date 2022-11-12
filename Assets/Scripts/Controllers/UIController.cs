using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public static UIController Instance {get; private set;}


    [SerializeField]
    private MenuWindow menuWindow;

    [SerializeField]
    private IngameUI ingameUI;

    [SerializeField]
    private GameObject gameOverWindow;

    [SerializeField]

    private GameObject gameOverWindowPve;

    [SerializeField]

    private Text winnerText, gameOverText;

    [SerializeField]

    private ArrowHint arrowHint;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameEventManager.GameStart += HideMenuWindow;
        GameEventManager.GameOver += ShowMenuWindow;

        GameEventManager.GameStart += ShowingameUI;
        GameEventManager.GameOver += HideingameUI;
    }

    public void HideMenuWindow()
    {
        menuWindow.gameObject.SetActive(false);
    }


    public void ShowMenuWindow()
    {
        HideAllWindows();
        menuWindow.gameObject.SetActive(true);
    }

    public void UpdateScore(int first, int second)
    {
        ingameUI.UpdateScore(first, second);
    }

    public void UpdateScoreFirst(int _score)
    {
        ingameUI.UpdateScoreFirst(_score);
    }

    public void UpdateScoreSecond(int _score)
    {
        ingameUI.UpdateScoreSecond(_score);
    }

    public void ShowGameOverWindow(BelongingType winner)
    {
        HideAllWindows();
        gameOverWindow.SetActive(true);
        if (winner == BelongingType.Player1)
        {
            gameOverText.color = Color.red;
            winnerText.color = Color.red;
            winnerText.text = "Player 1 is victorious!";
        }
        else
        {
            gameOverText.color = Color.blue;
            winnerText.color = Color.blue;
            winnerText.text = "Player 2 is victorious!";
        }
    }

    public void ShowArrowHint(bool addPlayer2)
    {
        arrowHint.ShowHint(addPlayer2);
    }

    public void HideGameOverWindow()
    {
        gameOverWindow.SetActive(false);
    }

    public void ShowGameOverWindowPve()
    {
        HideAllWindows();
        gameOverWindowPve.SetActive(true);
    }

    public void HideGameOverWindowPve()
    {
        gameOverWindowPve.SetActive(false);
    }
    private void HideAllWindows()
    {
        HideMenuWindow();
        HideGameOverWindow();
        HideGameOverWindowPve();
    }

    private void HideingameUI()
    {
        ingameUI.gameObject.SetActive(false);
    }

    private void ShowingameUI()
    {
        ingameUI.gameObject.SetActive(true);
    }
}
