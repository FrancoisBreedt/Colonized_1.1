using UnityEngine;
using UnityEngine.Rendering;

public class GameOver : MonoBehaviour
{

    [SerializeField] GameObject Text;
    readonly float WhiteOutTime = 2;
    readonly float TotalTime = 5;
    private float WhiteOutProgress = 0;

    void Update()
    {
        if (GameManager.instance.gameOver)
        {
            GetComponent<Volume>().weight = Mathf.Clamp(WhiteOutProgress / WhiteOutTime, 0, 1);
            WhiteOutProgress += Time.deltaTime;
            Text.SetActive(true);
            if (WhiteOutProgress >= TotalTime)
            {
                GameManager.instance.Restart();
            }
        }
    }
}
