using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    [SerializeField] Text HighScoreText;

    void Update()
    {
        GetComponent<Text>().text = Mathf.RoundToInt(GameManager.instance.Score).ToString();
        HighScoreText.text = Mathf.RoundToInt(GameManager.instance.HighScore).ToString();
    }
}
