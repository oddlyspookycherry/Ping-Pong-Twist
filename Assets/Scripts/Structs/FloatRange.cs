using UnityEngine;

[System.Serializable]
public struct FloatRange  
{
    public float min, max;

    public float RandomValue
    {
        get 
        {
            return Random.Range(min, max);
        }
    }
}
