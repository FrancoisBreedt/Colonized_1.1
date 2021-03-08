using UnityEngine;

public class AlienDie : StateMachineBehaviour
{

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 0.9f)
        {
            animator.gameObject.GetComponent<Health>().Die();
        }
    }

}
