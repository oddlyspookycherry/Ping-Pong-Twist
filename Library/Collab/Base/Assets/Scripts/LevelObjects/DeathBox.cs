using UnityEngine;

public class DeathBox : MonoBehaviour
{
    [SerializeField]
    private BelongingType belongingType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Ball ball = other.GetComponent<Ball>();
        if(ball != null)
        {
            ball.Deactivate();
            GameEventManager.TriggerBallScored(belongingType);
        }    
    }
}
