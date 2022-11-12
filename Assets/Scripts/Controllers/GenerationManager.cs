using UnityEngine;
using System.Collections.Generic;

public class GenerationManager : MonoBehaviour
{
    public static GenerationManager Instance {get; private set;}

    private List<Generatable>[] pools;

    [SerializeField]
    private Ball ball;

    [SerializeField] 

    private SpawnZone ballSpawnZone;

    [SerializeField]

    private GravityZone gravityZonePrefab;

    [SerializeField]

    private SpawnZone gravityZoneSpawnZone;

    [SerializeField]

    private Effect[] effects;

    void Awake() 
    {
        Instance = this;
        SetIndexes();
        CreatePools();
    }

    private void CreatePools()
    {
        pools = new List<Generatable>[effects.Length + 1];
        for(int i = 0; i < pools.Length; i++) {
            pools[i] = new List<Generatable>();
        }

    }

    private void SetIndexes()
    {
        gravityZonePrefab.poolIndex = 0;
        for(int i = 0; i < effects.Length; i++)
            effects[i].poolIndex = i + 1;
    }

    public void Reclaim(Generatable generatableToRecycle)
    {
        pools[generatableToRecycle.poolIndex].Add(generatableToRecycle);
        generatableToRecycle.gameObject.SetActive(false);
    }

    private T Generate<T>(T prefab) where T: Generatable
    {
        T generatable;
        List<Generatable> pool = pools[prefab.poolIndex];
        int lastIndex = pool.Count - 1;
        if(lastIndex >= 0)
        {
            generatable = pool[lastIndex] as T;
            generatable.gameObject.SetActive(true);
            pool.RemoveAt(lastIndex);
        }
        else
        {
            generatable = Instantiate(prefab);
        }
        generatable.poolIndex = prefab.poolIndex;
        generatable.isActive = true;
        
        return generatable;
    }

    public void GenerateBall(float speed, float incriment, bool Player1 = true)
    {
        Vector2 dir = new Vector2(Player1 ? 1f: -1f, Random.Range(0f, 1f) > 0.5f ? 1f: -1f);
        Vector2 pos = ballSpawnZone.point;
        if(!Player1)
            pos = new Vector2(-pos.x, pos.y);
        ball.gameObject.SetActive(true);
        ball.Initialize(pos, dir.normalized * speed, incriment);
    }

    public void GenerateGravityZone(float effectAmount, float maxTime)
    {
        Generate<GravityZone>(gravityZonePrefab).Initialize(gravityZoneSpawnZone.point, effectAmount, maxTime);
    }

    public void GenerateEffect(Vector2 pos, FXType type)
    {
        Generate<Effect>(effects[(int)type]).Initialize(pos);
    }
}
