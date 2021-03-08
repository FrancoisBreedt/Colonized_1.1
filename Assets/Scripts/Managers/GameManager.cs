using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject[] AssetsToSpawn;
    public GameObject Base;
    public List<List<GameObject>> RockObjects = new List<List<GameObject>>();
    public List<List<GameObject>> AssetList = new List<List<GameObject>>();
    public int[] Prices;
    public float[] Resources;
    public float Score = 0;
    public float HighScore = 0;
    public int StorageLevel = 1;
    public int[] AssetLevels = {1, 1, 1, 1};
    public bool CrystalStorageFull = false;
    public bool RockStorageFull = false;
    public bool gameOver = false;
    
    public static GameManager instance;

    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        for (int i = 0; i < AssetsToSpawn.Length; i++)
        {
            AssetList.Add(new List<GameObject>());
        }
        LoadGame();
    }

    void Update()
    {
        for (int i = 0; i < RockObjects.Count; i++)
        {
            for (int j = 0; j < RockObjects[i].Count; j++)
            {
                if (!RockObjects[i][j].GetComponent<Rock>().Mineable)
                {
                    RockObjects[i].RemoveAt(j);
                }
            }
        }
        for (int i = 0; i < AssetList.Count; i++)
        {
            for (int j = 0; j < AssetList[i].Count; j++)
            {
                if (AssetList[i][j].gameObject == null)
                {
                    AssetList[i].RemoveAt(j);
                }
            }
        }
    }

    public void Upgrade(int index)
    {
        Resources[1] -= Prices[index];
        Prices[index] += 2;
        AssetLevels[index]++;
        SaveGame();
    }

    public void Buy(int index)
    {
        Resources[0] -= Prices[index];
        Prices[index] += 1;
        AssetList[index].Add(Instantiate(AssetsToSpawn[index]));
        SaveGame();
    }

    public void GameOver()
    {
        gameOver = true;
    }

    public void Restart()
    {
        if (Score > HighScore)
        {
            HighScore = Score;
        }
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("HighScore", HighScore);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        SaveGame();
        Application.Quit();
    }

    [ContextMenu("Save")]
    void SaveGame()
    {
        PlayerPrefs.SetFloat("HighScore", HighScore);
    }

    [ContextMenu("Load")]
    void LoadGame()
    {
        HighScore = PlayerPrefs.GetFloat("HighScore", 0);
    }

}
