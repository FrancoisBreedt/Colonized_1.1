using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

    [SerializeField] List<Material> Materials;
    [SerializeField] GameObject Plane;
    [SerializeField] GameObject MainBase;
    [SerializeField] GameObject Rocks;
    [SerializeField] GameObject Crystals;
    [SerializeField] Vector3 StartPos;
    [SerializeField] Vector2 Dimentions;
    [SerializeField] float Scale;

    readonly List<List<List<float>>> MaterialWeights = new List<List<List<float>>>();
    readonly List<List<int>> MaterialMap = new List<List<int>>();

    [ContextMenu("Generate")]
    void GenerateTerrain()
    {
        GenerateWeights();
        FillList();
        PlacePlanes();
    }

    void FillList()
    {
        MaterialMap.Clear();
        for (int i = 0; i < Dimentions[0]; i++)
        {
            MaterialMap.Add(new List<int>());
            for (int j = 0; j < Dimentions[1]; j++)
            {
                MaterialMap[i].Add(RandomByWeight(MaterialWeights[i][j]));
            }
        }
    }

    void PlacePlanes()
    {
        for (int i = 0; i < Dimentions[0]; i++)
        {
            for (int j = 0; j < Dimentions[1]; j++)
            {
                Vector3 Pos = new Vector3(StartPos[0] + Scale * i, 0, StartPos[1] + Scale * j);
                GameObject p = Instantiate(Plane, Pos, Quaternion.identity);
                p.GetComponent<MeshRenderer>().material = Materials[MaterialMap[i][j]];
            }
        }
    }

    void GenerateWeights()
    {
        MaterialWeights.Clear();
        for (int i = 0; i < Dimentions[0]; i++)
        {
            MaterialWeights.Add(new List<List<float>>());
            for (int j = 0; j < Dimentions[1]; j++)
            {
                MaterialWeights[i].Add(new List<float>());
            }
        }
        Vector2[] GrassSpots = new Vector2[Mathf.CeilToInt(Dimentions[0] * Dimentions[1] * 0.001f)];
        Vector3[] Furthest = new Vector3[GrassSpots.Length]; // Index, value
        for (int i = 0; i < GrassSpots.Length; i++)
        {
            GrassSpots[i] = new Vector2(Random.Range(0, Dimentions[0]), Random.Range(0, Dimentions[1]));
            Furthest[i] = Vector3.zero;
        }
        for (int i = 0; i < Dimentions[0]; i++)
        {
            for (int j = 0; j < Dimentions[1]; j++)
            {
                float Length = 0;
                for (int k = 0; k < GrassSpots.Length; k++)
                {
                    Length += Vector2.Distance(new Vector2(i, j), GrassSpots[k]);
                }
                Length /= GrassSpots.Length;
                MaterialWeights[i][j].Add(Length);
                for (int k = 0; k < Furthest.Length; k++)
                {
                    if (Furthest[k].z < Length)
                    {
                        Furthest[k] = new Vector3(i, j, Length);
                    }
                }
            }
        }
        for (int i = 0; i < Dimentions[0]; i++)
        {
            for (int j = 0; j < Dimentions[1]; j++)
            {
                float Length = 0;
                for (int k = 0; k < Furthest.Length; k++)
                {
                    Length += Vector2.Distance(new Vector2(i, j), Furthest[k]);
                }
                Length /= Furthest.Length;
                MaterialWeights[i][j].Add(Length);
                for (int k = 2; k < Materials.Count; k++)
                {
                    MaterialWeights[i][j].Add(Random.Range(0, Vector2.Distance(Vector2.zero, Dimentions) * 10));
                }
            }
        }
        float max = 0;
        for (int i = 0; i < MaterialWeights.Count; i++)
        {
            for (int j = 0; j < MaterialWeights[0].Count; j++)
            {
                for (int k = 0; k < MaterialWeights[0][0].Count; k++)
                {
                    if (MaterialWeights[i][j][k] > max)
                    {
                        max = MaterialWeights[i][j][k];
                    }
                }
            }
        }
        for (int i = 0; i < MaterialWeights.Count; i++)
        {
            for (int j = 0; j < MaterialWeights[0].Count; j++)
            {
                for (int k = 0; k < MaterialWeights[0][0].Count; k++)
                {
                    MaterialWeights[i][j][k] = max - MaterialWeights[i][j][k];
                }
            }
        }
    }

    int RandomByWeight(List<float> values)
    {
        for (int i = 1; i < values.Count; i++)
        {
            values[i] += values[i - 1];
        }
        float randNum = Random.Range(0.0f, values[values.Count - 1]);
        for (int i = 0; i < values.Count; i++)
        {
            if (values[i] > randNum)
            {
                return i;
            }
        }
        return 0;
    }

}
