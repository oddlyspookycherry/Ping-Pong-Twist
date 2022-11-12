using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Wall : MonoBehaviour
{
    [SerializeField]

    private float minDelay, maxDelay, minTravelTime, maxTravelTime;

    private Transform gleam;

    private Vector3 startPos, endPos;

    void Start()
    {
        if(maxTravelTime < minDelay)
        {
            gleam = transform.GetChild(0);

            startPos = gleam.localPosition;
            endPos = new Vector3(-startPos.x, startPos.y, 0f);

            StartCoroutine(Pulse());
        }
        else
        {
            Debug.LogError("Invalid delay.");
        }
    }

    IEnumerator Pulse()
    {
        while(true)
        {
            float travelTime = Random.Range(minTravelTime, maxTravelTime);
            float delay = Random.Range(minDelay, maxDelay);

            gleam.localPosition = startPos;
            gleam.DOLocalMove(endPos, travelTime);

            yield return new WaitForSeconds(delay);
        }
    }
}
