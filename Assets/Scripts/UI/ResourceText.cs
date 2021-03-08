using UnityEngine;
using UnityEngine.UI;

public class ResourceText : MonoBehaviour
{

    [SerializeField] GameObject CrystalText;
    [SerializeField] GameObject RockText;

    void Update()
    {
        CrystalText.GetComponent<Text>().text = "Crystals: " + Mathf.Floor(GameManager.instance.Resources[0]).ToString();
        RockText.GetComponent<Text>().text =    "Rocks:    " + Mathf.Floor(GameManager.instance.Resources[1]).ToString();
    }
}
