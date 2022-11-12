using UnityEngine;
using System.Collections;

public partial class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; private set; }

    [SerializeField]

    private Ball ball;

    public Vector2 ballPosition
    {
        get
        {
            return ball.transform.position;
        }
    }

    [SerializeField]

    private Platform platform1, platform2;

    [SerializeField]

    private ImageEffect imageEffect;

    [SerializeField]

    private BlurFrame blurFrame;

    [SerializeField]
    private float ballStartSpeed, ballSpeedIncriment;

    [SerializeField]
    private int pvpVictoryScore = 10;

    [SerializeField]

    public float[] speedKeys; 

    [SerializeField, Range(0f, 1f)]
    private float slowMoAmount, platformSpeedScale;

    [SerializeField]

    private float slowMoWindowDuration, slowMoWindowInterval;

    [SerializeField]

    private float gravityZoneEffectAmount, gravityZoneDuration, gravityZoneSpawnInterval;

    public GameType gameType { get; set; }

    private int _firstScore;
    private int _secondScore;

    private int currentPhase;

    private bool player1Serve;

    public int firstScore
    {
        get => _firstScore;
        set
        {
            _firstScore = value;
            UIController.Instance.UpdateScore(_firstScore, _secondScore);
        }
    }

    public int secondScore
    {
        get => _secondScore;
        set
        {
            _secondScore = value;
            UIController.Instance.UpdateScore(_firstScore, _secondScore);
        }
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameEventManager.GameStartTyped += GameStart;
        GameEventManager.BallScored += OnBallScored;
        UIController.Instance.ShowMenuWindow();
    }

#if UNITY_EDITOR

    //Fast Forward
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            platform1.AiControlled = true;
            platform2.AiControlled = true;
            TimeManager.timeScale = 2f;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            platform1.AiControlled = false;
            if(gameType != GameType.Pve)
                platform2.AiControlled = false;
            TimeManager.timeScale = 1f;
        }
    }

#endif

    public void GameStart(GameType newGameType)
    {
        gameType = newGameType;
        player1Serve = true;

        if (gameType == GameType.Pvp)
        {
            platform2.AiControlled = false;
            firstScore = secondScore = 0;
            UIController.Instance.ShowArrowHint(true);
        }
        else
        {
            platform2.AiControlled = true;
            firstScore = PlayerPrefs.GetInt("PlayerScore", 0);
            secondScore = PlayerPrefs.GetInt("AIScore", 0);
            UIController.Instance.ShowArrowHint(false);
        }

        UnlockPlatformControl();
        GenerationManager.Instance.GenerateBall(ballStartSpeed, ballSpeedIncriment);
    }

    private void OnBallScored(BelongingType belongingType)
    {
        GenerationManager.Instance.GenerateEffect(ballPosition, FXType.Death);

        RemoveAllPhases();

        if (belongingType == BelongingType.Player1)
        {
            secondScore++;
        }
        else
        {
            firstScore++;
        }

        if (gameType == GameType.Pve)
        {
            LockPlatformControl();
            PlayerPrefs.SetInt("AIScore", Mathf.Clamp(secondScore, 0, 99));
            PlayerPrefs.SetInt("PlayerScore", Mathf.Clamp(firstScore, 0, 99));
            Invoke("FinishGame", 0.5f);
            return;
        }
        else
        {
            if (firstScore == pvpVictoryScore || secondScore == pvpVictoryScore)
            {
                LockPlatformControl();
                UIController.Instance.ShowGameOverWindow(belongingType);
                Invoke("FinishGame", 2f);
                return;
            }
            player1Serve = !player1Serve;
        }

        LockPlatformControl();
        Invoke("UnlockPlatformControl", 0.5f);
        Invoke("GenerateBall", 0.5f);
    }

    public void GenerateBall()
    {
        GenerationManager.Instance.GenerateBall(ballStartSpeed, ballSpeedIncriment, player1Serve);
    }

    public void PveVictory()
    {
        LockPlatformControl();
        RemoveAllPhases();
        UIController.Instance.ShowGameOverWindowPve();
        Invoke("FinishGame", 2f);
    }

    public void AddPhase(int phaseindex)
    {
        switch (phaseindex)
        {
            case 1:
                currentPhase = 1;
                StartCoroutine(Phase1());
                break;
            case 2:
                currentPhase = 2;
                StartCoroutine(Phase2());
                break;
            case 3:
                currentPhase = 3;
                imageEffect.enabled = true;
                ActivateSparks(false);
                break;
            default:
                Debug.LogError("Phase with such index does not exist.");
                break;
        }
    }

    IEnumerator Phase1()
    {
        while (true)
        {
            GenerationManager.Instance.GenerateGravityZone(gravityZoneEffectAmount, gravityZoneDuration);
            yield return new WaitForSeconds(gravityZoneSpawnInterval);
        }
    }

    IEnumerator Phase2()
    {
        while (true)
        {
            TimeManager.timeScale = slowMoAmount;
            TimeManager.platformSpeedScale = platformSpeedScale;
            blurFrame.Activate();
            if (currentPhase < 3)
                ActivateSparks(true);

            yield return new WaitForSeconds(slowMoWindowDuration);

            TimeManager.timeScale = 1f;
            TimeManager.platformSpeedScale = 1f;
            blurFrame.Deactivate();
            if (currentPhase < 3)
                DeactivateSparks();

            yield return new WaitForSeconds(slowMoWindowInterval);
        }
    }

    public void RemoveAllPhases()
    {
        StopAllCoroutines();
        currentPhase = 0;
        TimeManager.timeScale = 1f;
        imageEffect.enabled = false;
        DeactivateSparks();
    }

    private void ActivateSparks(bool isSlow)
    {
        platform1.ActivateSparks(isSlow);
        platform2.ActivateSparks(isSlow);
    }

    private void DeactivateSparks()
    {
        platform1.DeactivateSparks();
        platform2.DeactivateSparks();
    }

    private void LockPlatformControl()
    {
        platform1.controlable = false;
        platform2.controlable = false;
    }

    private void UnlockPlatformControl()
    {
        platform1.controlable = true;
        platform2.controlable = true;
    }

    private void FinishGame()
    {
        GameEventManager.TriggerGameOver();
    }
}
