using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{

    [SerializeField] List<GameObject> Rocks;
    [SerializeField] int InitialAmount;
    [SerializeField] int MaxRocks;
    [SerializeField] float TimeBetweenSpawn;
    [SerializeField] Vector2 RadiusBounds;

    float SpawnProgress = 0;
    int TotalRocks = 0;

    void Start()
    {
        for (int i = 0; i < Rocks.Count; i++)
        {
            GameManager.instance.RockObjects.Add(new List<GameObject>());
        }
        for (int i = 0; i < InitialAmount; i++)
        {
            Spawn();
        }
    }

    void Update()
    {
        SpawnProgress += Time.deltaTime;
        if (SpawnProgress > TimeBetweenSpawn)
        {
            SpawnProgress = 0;
            if (TotalRocks < MaxRocks)
            {
                Spawn();
            }
        }
    }

    void Spawn()
    {
        int RockIndex = Random.Range(0, Rocks.Count);
        Vector2 unitV = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        float Radius = Random.Range(RadiusBounds[0], RadiusBounds[1]);
        Vector3 SpawnPos = new Vector3(unitV.x * Radius, 0, unitV.y * Radius);
        GameManager.instance.RockObjects[RockIndex].Add(Instantiate(Rocks[RockIndex], SpawnPos, Quaternion.identity));
        GameObject G = GameManager.instance.RockObjects[RockIndex][GameManager.instance.RockObjects[RockIndex].Count - 1];
        float Health = Random.Range(5f, 50);
        G.GetComponent<Rock>().InitialHP = Health;
        G.GetComponent<Rock>().HP = Health;
        G.GetComponent<Rock>().StartingScale = Health / 5;
        G.transform.localScale = Vector3.one * (Health / 5);
        G.transform.RotateAround(Vector3.up, Random.Range(0.0f, 360));
        TotalRocks = 0;
        for (int i = 0; i < GameManager.instance.RockObjects.Count; i++)
        {
            for (int j = 0; j < GameManager.instance.RockObjects[i].Count; j++)
            {
                TotalRocks++;
            }
        }
    }
}
