using UnityEngine;

public class RectangleSpawnZone : SpawnZone
{
    [SerializeField]
    private float zone_Size_x, zone_Size_y;

    public override Vector2 point 
    {
        get {
            float x = Random.Range(-zone_Size_x, zone_Size_x);
            float y = Random.Range(-zone_Size_y, zone_Size_y);
            return new Vector2(x, y) + (Vector2)transform.position;
        }
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Vector2 p1 = new Vector2(-zone_Size_x, -zone_Size_y) + (Vector2)transform.position;
        Vector2 p2 = new Vector2(-zone_Size_x, zone_Size_y) + (Vector2)transform.position;
        Vector2 p3 = new Vector2(zone_Size_x, zone_Size_y) + (Vector2)transform.position;
        Vector2 p4 = new Vector2(zone_Size_x, -zone_Size_y) + (Vector2)transform.position;
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);
    }
    #endif
}
