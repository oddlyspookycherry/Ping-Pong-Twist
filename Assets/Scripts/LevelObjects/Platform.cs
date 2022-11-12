using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]

    private float speed;

    [SerializeField]

    private BelongingType belongingType;

    [SerializeField]
    private InputCollider inputCollider;

    private bool isMoving { get; set; }

    private Rigidbody2D rb2d;

    [SerializeField]
    private float limit;

    public bool controlable { get; set; }


    public bool AiControlled { get; set; }

    private delegate void PCControlAction();

    private PCControlAction ControlAction;

    private void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if(belongingType == BelongingType.Player1)
            ControlAction = ControlPlayer1;
        else
            ControlAction = ControlPlayer2;
    }

    public void ActivateSparks(bool isSlow)
    {
        if(isSlow)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(true);
        }
    }

    public void DeactivateSparks()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }

    private void ControlPlayer1()
    {
        int sign = 0;

        if (Input.GetKey(KeyCode.W))
        {
            sign = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            sign = -1;
        }

        float delta = sign * speed * Time.deltaTime * TimeManager.platformSpeedScale;
        if (Mathf.Abs(rb2d.position.y + delta) <= limit)
            rb2d.position += new Vector2(0f, delta);
    }

    private void ControlPlayer2()
    {
        int sign = 0;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            sign = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            sign = -1;
        }

        float delta = sign * speed * Time.deltaTime * TimeManager.platformSpeedScale;
        if (Mathf.Abs(rb2d.position.y + delta) <= limit)
            rb2d.position += new Vector2(0f, delta);
    }

    void Update()
    {
        if (!controlable)
            return;

        if (AiControlled)
        {
            Vector2 pos = LevelController.Instance.ballPosition;
            if (Mathf.Abs(pos.y) <= limit)
                rb2d.position = new Vector2(rb2d.position.x, pos.y);
            return;
        }

#if UNITY_EDITOR

        ControlAction();

#else

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            if(touch.phase == TouchPhase.Began && !inputCollider.IsInsideCollider(touchPos))
                isMoving = false;

            if(inputCollider.IsInsideCollider(touchPos))
                isMoving = true;

            if(isMoving)       
                rb2d.velocity = new Vector2(0f, (touchPos.y - rb2d.position.y)) * speed * TimeManager.platformSpeedScale;
        }
        else
        {
            rb2d.velocity = Vector2.zero;
        }

#endif
    }
}
