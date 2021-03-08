using UnityEngine;
using UnityEngine.UI;

public class TimeSelect : MonoBehaviour
{

    [SerializeField] Button Button;
    [SerializeField] Sprite[] Images;

    int CurrentMode = 1;

    public void ChangeTime()
    {
        CurrentMode = ++CurrentMode % 3;
        Button.image.sprite = Images[CurrentMode];
        Time.timeScale = CurrentMode;
    }
}
