using System;
using System.Collections.Generic;
using UnityEngine;

public class Miner : MonoBehaviour
{

    [SerializeField] List<int> ObjectsToGet;
    [SerializeField] float StopDistance;
    [SerializeField] string MinerType;

    GameObject FollowObject;
    private float RocksOnBoard = 0;
    float MineStrength;
    float MaxRocks;
    float MovementSpeed;
    float TurnSpeed;

    Vector3 RandomPos = Vector3.zero;

    void Update()
    {
        if (MinerType == "Crystal")
        {
            MovementSpeed = GameManager.instance.AssetLevels[0] * 5 + 5;
            MineStrength = GameManager.instance.AssetLevels[0];
            MaxRocks = GameManager.instance.AssetLevels[0] * 5;
        }
        else
        {
            MovementSpeed = GameManager.instance.AssetLevels[1] * 5 + 5;
            MineStrength = GameManager.instance.AssetLevels[1];
            MaxRocks = GameManager.instance.AssetLevels[1] * 5;
        }
        TurnSpeed = MovementSpeed;
        if (RocksOnBoard >= MaxRocks)
        {
            ReturnToBase();
        }
        else if ((MinerType == "Crystal" && GameManager.instance.CrystalStorageFull) ||
            (MinerType == "Rock" && GameManager.instance.RockStorageFull))
        {
            FlyRandom();
        }
        else
        {
            try
            {
                Mine();
            }
            catch (Exception)
            {
                FlyRandom();
            }
        }
    }

    private void GetFollowObject()
    {
        GameManager GM = GameManager.instance;
        List<List<GameObject>> Rocks = GM.RockObjects;
        float[] ClosestDist = new float[5] { -1, -1, -1, -1, -1 };
        List<int[]> ObjectIndeces = new List<int[]>() { new int[2], new int[2], new int[2], new int[2], new int[2]};
        for (int i = 0; i < Rocks.Count; i++)
        {
            for (int j = 0; j < ObjectsToGet.Count; j++)
            {
                if (ObjectsToGet[j] == i)
                {
                    for (int k = 0; k < Rocks[i].Count; k++)
                    {
                        for (int l = 0; l < ClosestDist.Length; l++)
                        {
                            if (ClosestDist[l] < 0 || Vector3.Distance(GetComponent<Transform>().position, Rocks[i][k].GetComponent<Transform>().position) < ClosestDist[l])
                            {
                                ObjectIndeces[l] = new int[2] { i, k };
                                ClosestDist[l] = Vector3.Distance(GetComponent<Transform>().position, Rocks[i][k].GetComponent<Transform>().position);
                                break;
                            }
                        }
                    }
                }
            }
        }
        int R = UnityEngine.Random.Range(0, ClosestDist.Length);
        FollowObject = Rocks[ObjectIndeces[R][0]][ObjectIndeces[R][1]];
    }

    void Mine()
    {
        if (FollowObject == null)
        {
            GetFollowObject();
        }
        else if (FollowObject.GetComponent<Rock>().Mineable)
        {
            Vector3 Pos = GetComponent<Transform>().position;
            Vector3 MoveDir = FollowObject.GetComponent<Transform>().position - Pos;
            MoveDir.y = 0;
            MoveDir.Normalize();
            Vector3 NewForward = Vector3.RotateTowards(GetComponent<Transform>().forward, MoveDir, TurnSpeed * Time.deltaTime, 1);
            GetComponent<Transform>().forward = NewForward;
            if (Vector3.Distance(FollowObject.transform.position + new Vector3(0, Pos.y - FollowObject.transform.position.y, 0), transform.position) > StopDistance)
            {
                GetComponent<Transform>().position = Vector3.MoveTowards(Pos, Pos + NewForward * 1000, MovementSpeed * Time.deltaTime);
            }
            else
            {
                float x = FollowObject.GetComponent<Rock>().Mine(MineStrength * Time.deltaTime);
                RocksOnBoard += x;
                GameManager.instance.Score += x;
                GetComponent<Health>().health -= Time.deltaTime / 10;
            }
        }
        else
        {
            GetFollowObject();
        }
    }

    void ReturnToBase()
    {
        Vector3 Pos = GetComponent<Transform>().position;
        Vector3 MoveDir = Vector3.zero - Pos;
        MoveDir.y = 0;
        MoveDir.Normalize();
        Vector3 NewForward = Vector3.RotateTowards(GetComponent<Transform>().forward, MoveDir, TurnSpeed * Time.deltaTime, 1);
        GetComponent<Transform>().forward = NewForward;
        if (Vector3.Distance(new Vector3(0, Pos.y, 0), transform.position) <= StopDistance)
        {
            if (MinerType == "Crystal")
            {
                GameManager.instance.Resources[0] += RocksOnBoard;
            }
            else
            {
                GameManager.instance.Resources[1] += RocksOnBoard;
            }
            RocksOnBoard = 0;
        }
        GetComponent<Transform>().position = Vector3.MoveTowards(Pos, Pos + NewForward * 1000, MovementSpeed * Time.deltaTime);
    }

    void FlyRandom()
    {
        Vector3 Pos = GetComponent<Transform>().position;
        Vector3 MoveDir = RandomPos - Pos;
        MoveDir.y = 0;
        MoveDir.Normalize();
        Vector3 NewForward = Vector3.RotateTowards(GetComponent<Transform>().forward, MoveDir, TurnSpeed * Time.deltaTime, 1);
        GetComponent<Transform>().forward = NewForward;
        if (Vector3.Distance(RandomPos, transform.position) <= StopDistance * 2)
        {
            RandomPos = new Vector3(UnityEngine.Random.Range(-30, 30), Pos.y, UnityEngine.Random.Range(-30, 30));
        }
        GetComponent<Transform>().position = Vector3.MoveTowards(Pos, Pos + NewForward * 1000, MovementSpeed * Time.deltaTime * 0.5f);
    }

}
