using System.Collections.Generic;
using UnityEngine;

public class CanisterSpawner : MonoBehaviour
{

    [SerializeField] GameObject CrystalCanister;
    [SerializeField] GameObject RockCanister;
    [SerializeField] int MaxDim;
    [SerializeField] float Offset;

    readonly List<GameObject> CrystalCanisters = new List<GameObject>();
    readonly List<GameObject> RockCanisters = new List<GameObject>();

    int StorageLevel = 1;
    float Spacing;
    float Capacity;

    void Update()
    {
        if (GameManager.instance.StorageLevel != StorageLevel)
        {
            StorageLevel = GameManager.instance.StorageLevel;
            DestroyAll();
        }
        Capacity = GameManager.instance.StorageLevel * 10;
        Spacing = (GameManager.instance.StorageLevel + 2) * 0.2f;
        float x = Offset + MaxDim * Spacing - (CrystalCanisters.Count % MaxDim) * Spacing;
        float z = Mathf.Floor(MaxDim / 2) * Spacing - Mathf.Floor(CrystalCanisters.Count / MaxDim) * Spacing;
        Vector3 Pos = new Vector3(x, 0, z);
        if ((GameManager.instance.Resources[0] / Capacity) > CrystalCanisters.Count && CrystalCanisters.Count < (MaxDim * MaxDim))
        {
            CrystalCanisters.Add(Instantiate(CrystalCanister, Pos, Quaternion.identity));
            CrystalCanisters[CrystalCanisters.Count - 1].transform.localScale = Vector3.one * (GameManager.instance.StorageLevel + 2);
        }
        if ((GameManager.instance.Resources[0] / Capacity) <= (CrystalCanisters.Count - 1) && CrystalCanisters.Count > 0)
        {
            Destroy(CrystalCanisters[CrystalCanisters.Count - 1]);
            CrystalCanisters.RemoveAt(CrystalCanisters.Count - 1);
        }
        x = -Offset - MaxDim * Spacing + (RockCanisters.Count % MaxDim) * Spacing;
        z = Mathf.Floor(MaxDim / 2) * Spacing - Mathf.Floor(RockCanisters.Count / MaxDim) * Spacing;
        Pos = new Vector3(x, 0, z);
        if ((GameManager.instance.Resources[1] / Capacity) > RockCanisters.Count && RockCanisters.Count < (MaxDim * MaxDim))
        {
            RockCanisters.Add(Instantiate(RockCanister, Pos, Quaternion.identity));
            RockCanisters[RockCanisters.Count - 1].transform.localScale = Vector3.one * (GameManager.instance.StorageLevel + 2);
        }
        if ((GameManager.instance.Resources[1] / Capacity) <= (RockCanisters.Count - 1) && RockCanisters.Count > 0)
        {
            Destroy(RockCanisters[RockCanisters.Count - 1]);
            RockCanisters.RemoveAt(RockCanisters.Count - 1);
        }
        GameManager.instance.CrystalStorageFull = GameManager.instance.Resources[0] >= (Capacity * MaxDim * MaxDim);
        GameManager.instance.RockStorageFull = GameManager.instance.Resources[1] >= (Capacity * MaxDim * MaxDim);
    }

    void DestroyAll()
    {
        foreach (var obj in CrystalCanisters)
        {
            Destroy(obj);
        }
        CrystalCanisters.Clear();
        foreach (var obj in RockCanisters)
        {
            Destroy(obj);
        }
        RockCanisters.Clear();
    }

}
