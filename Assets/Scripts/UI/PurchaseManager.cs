using UnityEngine;
using UnityEngine.UI;

public class PurchaseManager : MonoBehaviour
{

    [SerializeField] Text[] PriceText;
    [SerializeField] Text[] QuantityText;
    [SerializeField] GameObject[] UpgradeButton;
    [SerializeField] GameObject[] AddButton;
    [SerializeField] Color CrystalColor;
    [SerializeField] Color RockColor;

    void Start()
    {
        UpdatePriceText();
        for (int i = 0; i < PriceText.Length; i++)
        {
            UpgradeButton[i].GetComponent<Image>().color = RockColor;
            AddButton[i].GetComponent<Image>().color = CrystalColor;
        }
    }

    void Update()
    {
        for (int i = 0; i < PriceText.Length; i++)
        {
            UpgradeButton[i].SetActive(GameManager.instance.Resources[1] >= GameManager.instance.Prices[i]);
            AddButton[i].SetActive(GameManager.instance.Resources[0] >= GameManager.instance.Prices[i]);
            QuantityText[i].text = GameManager.instance.AssetList[i].Count.ToString();
        }
    }

    void UpdatePriceText()
    {
        for (int i = 0; i < PriceText.Length; i++)
        {
            PriceText[i].text = GameManager.instance.Prices[i].ToString();
        }
    }

    public void Upgrade(int index)
    {
        GameManager.instance.Upgrade(index);
        UpdatePriceText();
    }

    public void Buy(int index)
    {
        GameManager.instance.Buy(index);
        UpdatePriceText();
    }
}
