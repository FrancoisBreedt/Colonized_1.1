using System.Collections.Generic;
using UnityEngine;

public class AlienWalk : StateMachineBehaviour
{

    [SerializeField] float MovementSpeed;
    [SerializeField] float TurnSpeed;
    [SerializeField] float StopDistance;

    GameObject Base;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Base = GameManager.instance.Base;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        List<List<GameObject>> AssetList = GameManager.instance.AssetList;
        GameObject gameObject = animator.gameObject;
        Vector3 OwnPos = gameObject.transform.position;
        float ShortestDist = Vector3.Distance(OwnPos, Vector3.zero);
        GameObject ClosestObject = Base;
        // Search for closest object to damage
        for (int i = 0; i < AssetList.Count; i++)
        {
            for (int j = 0; j < AssetList[i].Count; j++)
            {
                if (AssetList[i][j].gameObject != null)
                {
                    Vector3 tempObjectPos = AssetList[i][j].transform.position;
                    if (ShortestDist > Vector3.Distance(OwnPos, tempObjectPos))
                    {
                        ShortestDist = Vector3.Distance(OwnPos, tempObjectPos);
                        ClosestObject = AssetList[i][j];
                    }
                }
            }
        }
        Vector3 MoveDir = ClosestObject.transform.position - OwnPos;
        MoveDir.y = 0;
        MoveDir.Normalize();
        Vector3 NewForward = Vector3.RotateTowards(gameObject.transform.forward, MoveDir, TurnSpeed * Time.deltaTime, 1);
        gameObject.GetComponentInParent<Transform>().forward = NewForward;
        if (ShortestDist > StopDistance)
        {
            gameObject.GetComponentInParent<Transform>().position = Vector3.MoveTowards(OwnPos, OwnPos + NewForward * 1000, MovementSpeed * Time.deltaTime);
        }
        else
        {   
            animator.SetBool("Punching", true); 
            animator.SetBool("Walking", false);
        }
    }

}
