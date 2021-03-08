using UnityEngine;

public class PersonWalk : StateMachineBehaviour
{

    [SerializeField] float TurnSpeed;

    Vector3 Destination;
    float MovementSpeed;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MovementSpeed = Random.Range(0.5f, 2);
        animator.SetFloat("Speed", MovementSpeed / 2);
        Destination = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * Random.Range(10, 40);
        animator.SetBool("Look", false);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject gameObject = animator.gameObject;
        Vector3 OwnPos = gameObject.transform.position;
        Vector3 MoveDir = Destination - OwnPos;
        MoveDir.y = 0;
        MoveDir.Normalize();
        Vector3 NewForward = Vector3.RotateTowards(gameObject.transform.forward, MoveDir, TurnSpeed * Time.deltaTime, 1);
        gameObject.transform.forward = NewForward;
        if (Vector3.Distance(Destination, OwnPos) > 2)
        {
            gameObject.transform.position = Vector3.MoveTowards(OwnPos, OwnPos + NewForward * 1000, MovementSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Look", true);
        }
        GameObject[] Aliens = FindObjectsOfType<GameObject>();
        for (int i = 0; i < Aliens.Length; i++)
        {
            if (Aliens[i].CompareTag("Enemy") && Vector3.Distance(Aliens[i].transform.position, Vector3.zero) < 60)
            {
                animator.SetBool("Panic", true);
            }
        }
    }

}
