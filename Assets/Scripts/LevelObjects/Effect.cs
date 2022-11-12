using UnityEngine;

public class Effect : Generatable
{
    public void Initialize(Vector2 pos)
    {
        transform.position = pos;
        Invoke("Recycle", GetComponent<ParticleSystem>().main.duration);
    }
}
