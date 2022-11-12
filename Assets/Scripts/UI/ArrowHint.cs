using UnityEngine;

public class ArrowHint : MonoBehaviour
{   
    [SerializeField]
    private Animation player1anim, player2anim;

    public void ShowHint(bool addPlayer2)
    {
        player1anim.Play();
        if(addPlayer2)
            player2anim.Play();
    }
}
