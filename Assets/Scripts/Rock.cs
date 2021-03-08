using UnityEngine;

public class Rock : MonoBehaviour
{

    public float InitialHP;
    public float StartingScale;

    public float HP;
    public bool Mineable = true;

    readonly float DestroyTime = 0.2f;
    private float DestroyProgress = 0;

    void Update()
    {
        if (!Mineable)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            DestroyProgress += Time.deltaTime;
        }
        if (DestroyProgress > DestroyTime)
        {
            Destroy(gameObject);
        }
    }

    public float Mine(float Strength)
    {
        transform.localScale = Vector3.one * (StartingScale) * (HP / InitialHP);
        if (HP - Strength <= 0)
        {
            HP = 0;
            Mineable = false;
            return HP;
        }
        else
        {
            HP -= Strength;
            return Strength;
        }
    }

}
