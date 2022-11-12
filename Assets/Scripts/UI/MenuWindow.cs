using UnityEngine;

public class MenuWindow : MonoBehaviour
{
    public void OnPvpButton()
    {
        GameEventManager.TriggerGameStart(GameType.Pvp);
    }

    public void OnPveButton()
    {
        GameEventManager.TriggerGameStart(GameType.Pve);
    }
}
