using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    [SerializeField] float StopDistance;

    GameObject FollowObject;
    float RepairStrength;
    float MovementSpeed;
    float TurnSpeed;

    Vector3 RandomPos = Vector3.zero;

    void Update()
    {
        MovementSpeed = GameManager.instance.AssetLevels[2] * 8 + 5;
        RepairStrength = GameManager.instance.AssetLevels[2];
        TurnSpeed = MovementSpeed;
        if (GetFollowObject() == null)
        {
            FlyRandom();
        }
        else
        {
            Repair();
        }
    }

    private GameObject GetFollowObject()
    {
        GameManager GM = GameManager.instance;
        List<List<GameObject>> Assets = GM.AssetList;
        float ClosestDist = -1;
        GameObject ClosestObject = null;
        // Decide which object to follow
        for (float x = 0.2f; x <= 1; x += 0.2f)
        {
            if (GM.Base.GetComponent<Health>().health < GM.Base.GetComponent<Health>().MaxHealth * x)
            {
                FollowObject = GM.Base;
                return GM.Base;
            }
            for (int i = 0; i < Assets.Count; i++)
            {
                for (int j = 0; j < Assets[i].Count; j++)
                {
                    if (Assets[i][j].GetComponent<Health>().health < Assets[i][j].GetComponent<Health>().MaxHealth * x && i != 3)
                    {
                        if (ClosestDist < 0 || Vector3.Distance(Assets[i][j].transform.position, transform.position) < ClosestDist)
                        {
                            ClosestObject = Assets[i][j];
                            ClosestDist = ClosestObject.GetComponent<Health>().health;
                        }
                    }
                }
            }
            if (ClosestObject != null)
            {
                break;
            }
        }
        FollowObject = ClosestObject;
        return ClosestObject;
    }

    void Repair()
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
            FollowObject.GetComponent<Health>().health += RepairStrength * Time.deltaTime;
            GetComponent<Health>().health -= Time.deltaTime;
        }
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
            RandomPos = new Vector3(Random.Range(-30, 30), Pos.y, Random.Range(-30, 30));
        }
        GetComponent<Transform>().position = Vector3.MoveTowards(Pos, Pos + NewForward * 1000, MovementSpeed * Time.deltaTime * 0.2f);
    }
}
