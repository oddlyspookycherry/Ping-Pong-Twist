using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Vector2 vel;

    private Vector3 startScale;

    private bool phaseOneCalled, phaseTwoCalled, phaseThreeCalled, pveFinalCalled;

    public Vector2 velocity 
    {
        get
        {
            return vel;
        }
        set
        {
            vel = value;
        }
    }

    private float incriment;

    private ContactFilter2D contactFilter;

    private RaycastHit2D[] hitBuffer = new RaycastHit2D[1];

    private IEnumerator burningCoroutine;

    private float[] speedKeys;

    private IEnumerator BurningCoroutine()
    {
        for(int i = 0; i < 300; i++)
        {
            transform.localScale = Mathf.Lerp(1f, 0f, i/299f) * Vector3.one;
            yield return null;
        }
        LevelController.Instance.PveVictory();
        Deactivate();
    }

    void OnEnable()
    {
        burningCoroutine = BurningCoroutine();
        rb2d = GetComponent<Rigidbody2D>();
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask (Physics2D.GetLayerCollisionMask (gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    public void Initialize(Vector2 pos, Vector2 vel, float incriment)
    {
        speedKeys = LevelController.Instance.speedKeys;
        rb2d.position = pos;
        this.vel = vel;
        this.incriment = incriment;
        startScale = transform.localScale;
        transform.GetChild(0).gameObject.SetActive(false);

        phaseOneCalled = false;
        phaseTwoCalled = false;
        phaseThreeCalled = false;
        pveFinalCalled = false;
    }

    public void Deactivate()
    {
        StopCoroutine(burningCoroutine);
        gameObject.SetActive(false);
        transform.localScale = startScale;
    }

    void Update()
    {
        float dist = (vel * Time.deltaTime).magnitude;

        if(rb2d.Cast(vel * Time.deltaTime, contactFilter, hitBuffer, dist) > 0)
        {
            GenerationManager.Instance.GenerateEffect(rb2d.position, FXType.Bounce);

            Vector2 normal = hitBuffer[0].normal;
            normal = new Vector2(normal.y, -normal.x);

            float dot = vel.x * normal.x + vel.y * normal.y;

            if(dot == 0)
            {
                vel *= -1f;
            }
            else
            {
                Vector2 projection = new Vector2(
                    vel.x * normal.x * normal.x + vel.y * normal.x * normal.y,
                    vel.x * normal.x * normal.y + vel.y * normal.y * normal.y
                    );
                vel /= normal.sqrMagnitude;
                vel = projection * 2f - vel;
                                    
            }
        }

        rb2d.position += vel * TimeManager.timeScale * Time.deltaTime;
        
        vel += vel.normalized * incriment * TimeManager.timeScale * Time.deltaTime;

        if(!phaseOneCalled && vel.magnitude > speedKeys[0])
        {
            phaseOneCalled = true;
            LevelController.Instance.AddPhase(1);
        }
        else if(!phaseTwoCalled && vel.magnitude > speedKeys[1])
        {
            phaseTwoCalled = true;
            LevelController.Instance.AddPhase(2);
        }
        else if(!phaseThreeCalled && vel.magnitude > speedKeys[2])
        {
            phaseThreeCalled = true;
            LevelController.Instance.AddPhase(3);
        }
        else if(LevelController.Instance.gameType == GameType.Pve && !pveFinalCalled && vel.magnitude > speedKeys[3])
        {
            pveFinalCalled = true;
            transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(burningCoroutine);
        }
    }
}
