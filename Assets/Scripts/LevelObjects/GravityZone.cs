using UnityEngine;
using DG.Tweening;

public class GravityZone : Generatable
{
    private float amount;
    private float maxTime;

    private float age;

    public void Initialize(Vector2 pos, float amount, float maxTime)
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.5f);

        transform.position = pos;
        this.amount = amount;
        this.maxTime = maxTime;
    }

    protected override void Recycle()
    {
        age = 0f; 
        Deactivate();
        transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => {
            GenerationManager.Instance.Reclaim(this);
        });
    }

    private void Update() 
    {
        if(age > maxTime)
        {
            Recycle();
        }
        age += Time.deltaTime;
    }


    private void OnTriggerStay2D(Collider2D other) 
    {
        if(!isActive)
            return;

        if(other.tag == "Ball")
        {
            Ball ball = other.GetComponent<Ball>();
            Vector2 velocity = ball.velocity;
            float magnitude = velocity.magnitude;
            Vector2 shift = (transform.position - other.transform.position).normalized * magnitude * amount
                * Time.deltaTime * TimeManager.timeScale;
            Vector2 newVel = (velocity + shift).normalized * magnitude;
            ball.velocity = newVel;
        }
    }
}
