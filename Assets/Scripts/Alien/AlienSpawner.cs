using UnityEngine;

public class AlienSpawner : MonoBehaviour
{
    [SerializeField] GameObject Alien;
    [SerializeField] float TimeBetweenSpawn;
    [SerializeField] Vector2 RadiusBounds;
    private float SpawnProgress = 0;

    void Update()
    {
        SpawnProgress += Time.deltaTime;
        if (SpawnProgress > TimeBetweenSpawn)
        {
            SpawnProgress = 0;
            int x = Mathf.FloorToInt(GameManager.instance.Score / 200);
            for (int i = 0; i < x; i++)
            {
                Spawn();
            }
        }
    }

    void Spawn()
    {
        Vector2 unitV = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        float Radius = Random.Range(RadiusBounds[0], RadiusBounds[1]);
        Vector3 SpawnPos = new Vector3(unitV.x * Radius, 0, unitV.y * Radius);
        Instantiate(Alien, SpawnPos, Quaternion.identity);
    }
}
