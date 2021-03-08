using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FadeIn : MonoBehaviour
{
    readonly float FadeInTime = 2;
    private float CurrentTime = 0;

    void Update()
    {
        GetComponent<Volume>().weight = 1 - CurrentTime / FadeInTime;
        CurrentTime += Time.deltaTime;
        if (CurrentTime > FadeInTime)
        {
            Destroy(this);
        }
    }
}
