using System.Collections.Generic;
using UnityEngine;

public class AlienPunch : StateMachineBehaviour
{

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
        // Find closest object to punch
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
                    }
                }
            }
        }
        if (ShortestDist > StopDistance)
        {
            animator.SetBool("Punching", false);
            animator.SetBool("Walking", true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("Punching"))
        {
            List<List<GameObject>> AssetList = GameManager.instance.AssetList;
            GameObject gameObject = animator.gameObject;
            Vector3 OwnPos = gameObject.transform.position;
            // Damage all close objects
            for (int i = 0; i < AssetList.Count; i++)
            {
                for (int j = 0; j < AssetList[i].Count; j++)
                {
                    if (AssetList[i][j].gameObject != null && Vector3.Distance(OwnPos, AssetList[i][j].transform.position) < StopDistance)
                    {
                        AssetList[i][j].GetComponent<Health>().health -= gameObject.GetComponent<Alien>().Damage; ;
                    }
                }
            }
            if (Vector3.Distance(OwnPos, Base.transform.position) < StopDistance)
            {
                Base.GetComponent<Health>().health -= gameObject.GetComponent<Alien>().Damage;
            }
        }
    }

}
