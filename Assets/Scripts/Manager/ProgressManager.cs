using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//进度管理器
public class ProgressManager : MonoBehaviour
{
    public int CrabAmount;
    public int EarthwormAmount;
    public int LoveAmount;

    private HashSet<int> foundCatIDs = new HashSet<int>();
    private int itemCount = 0;

    public int TotalCatCount => GameManager.Instance.catDatabase.GetAllCats().Count;
    public int FoundCatCount => foundCatIDs.Count;
    public int ItemCount => itemCount;


    public void Initialize()
    {
        foundCatIDs.Clear();
        itemCount = 0;
    }

    public void CatFound(int catID)
    {
        foundCatIDs.Add(catID);

        UIManager.Instance.UpdateProgressUI();
    }

    public bool IsCatFound(int catID)
    {
        return foundCatIDs.Contains(catID);
    }

    public int GetItemCount()
    {
        return itemCount;
    }


    public void AddItem()
    {
        itemCount++;
        UIManager.Instance.UpdateProgressUI();
        GameManager.Instance.conditionChecker.CheckConditions();
    }

    public void LoadProgress(int[] foundIDs, int items, int crab, int earthworm, int love)
    {
        foundCatIDs = new HashSet<int>(foundIDs);
        itemCount = items;
        CrabAmount = crab;
        EarthwormAmount = earthworm;
        LoveAmount = love;
    }

    public void UpdateItem(ItemType type, int value)
    {
        switch (type)
        {
            case ItemType.Crab: CrabAmount = value; break;
            case ItemType.Earthworm: EarthwormAmount = value; break;
            case ItemType.Love: LoveAmount = value; break;
        }
    }

    public int GetItem(ItemType type)
    {
        return type switch
        {
            ItemType.Crab => CrabAmount,
            ItemType.Earthworm => EarthwormAmount,
            ItemType.Love => LoveAmount,
            _ => 0
        };
    }

    public int[] GetFoundCatIDs()
    {
        return foundCatIDs.ToArray();
    }
}
