using UnityEngine;
using DG.Tweening;

public class BlurFrame : MonoBehaviour
{
    private Vector3 startScale;

    [SerializeField]
    private Vector3 activeScale;

    [SerializeField]

    private float activationTime;

    void Awake()
    {
        startScale = transform.localScale;
    }

    public void Activate()
    {
        transform.DOScale(activeScale, activationTime);
    }

    public void Deactivate()
    {
        transform.DOScale(startScale, activationTime);
    }
}
