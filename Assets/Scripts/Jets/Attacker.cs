using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] float StopDistance;

    GameObject FollowObject;
    float AttackStrength;
    float MovementSpeed;
    float TurnSpeed;

    Vector3 RandomPos = Vector3.zero;

    void Update()
    {
        MovementSpeed = GameManager.instance.AssetLevels[2] * 8 + 5;
        AttackStrength = GameManager.instance.AssetLevels[2] * 2;
        TurnSpeed = MovementSpeed;
        if (GetFollowObject() == null)
        {
            FlyRandom();
        }
        else
        {
            Attack();
        }
    }

    private GameObject GetFollowObject()
    {
        GameObject[] Aliens = GameObject.FindGameObjectsWithTag("Enemy");
        float ClosestDist = 80;
        GameObject ClosestObject = null;
        for (int i = 0; i < Aliens.Length; i++)
        {
            if (ClosestDist < 0 || Vector3.Distance(Aliens[i].transform.position, transform.position) < ClosestDist)
            {
                ClosestObject = Aliens[i];
                ClosestDist = Vector3.Distance(ClosestObject.transform.position, transform.position);
            }
        }
        FollowObject = ClosestObject;
        return ClosestObject;
    }

    void Attack()
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
            FollowObject.GetComponent<Health>().health -= AttackStrength * Time.deltaTime;
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
