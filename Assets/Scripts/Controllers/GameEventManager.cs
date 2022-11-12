public static class GameEventManager
{

    public delegate void GameEvent();

    public delegate void GameTypeEvent(GameType gameType);
    public delegate void BelongingTypeEvent(BelongingType belongingType);

    public static event BelongingTypeEvent BallScored;
    public static event GameEvent GameOver, GameStart;

    public static event GameTypeEvent GameStartTyped;

    public static void TriggerGameStart(GameType gameType)
    {
        if (GameStart != null)
            GameStart();
        if(GameStartTyped != null)
            GameStartTyped(gameType);
    }

    public static void TriggerGameOver()
    {
        if (GameOver != null)
        {
            GameOver();
        }
    }

    public static void TriggerBallScored(BelongingType belongingType)
    {
        if (BallScored != null)
        {
            BallScored(belongingType);
        }
    }
}