using UnityEngine;

public class RectangleInputCollider : InputCollider
{
    [SerializeField]
    private float Size_x, Size_y;

    public override bool IsInsideCollider(Vector2 point)
    {
        if(Mathf.Abs(transform.position.x - point.x) <= Size_x
            && Mathf.Abs(transform.position.y - point.y) <= Size_y)
                return true;
        return false;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Vector2 p1 = new Vector2(-Size_x, -Size_y) + (Vector2)transform.position;
        Vector2 p2 = new Vector2(-Size_x, Size_y) + (Vector2)transform.position;
        Vector2 p3 = new Vector2(Size_x, Size_y) + (Vector2)transform.position;
        Vector2 p4 = new Vector2(Size_x, -Size_y) + (Vector2)transform.position;
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);
    }
    #endif
}
