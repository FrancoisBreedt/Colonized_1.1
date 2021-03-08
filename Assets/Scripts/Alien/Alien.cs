using UnityEngine;

public class Alien : MonoBehaviour
{

    public float Damage = 5;

    public void Die()
    {
        GetComponent<Animator>().SetBool("Die", true);
        GetComponent<Animator>().SetBool("Punching", false);
    }

}
