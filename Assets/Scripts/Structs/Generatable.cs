using UnityEngine;

public class Generatable : MonoBehaviour
{

    public int poolIndex{get; set;}
    public bool isActive{get; set;}
    protected virtual void Recycle()
    {
        Deactivate();
        GenerationManager.Instance.Reclaim(this);
    }

    public void Activate()
    {
        isActive = true;
    }
    public void Deactivate()
    {
        isActive = false;
    }
}
