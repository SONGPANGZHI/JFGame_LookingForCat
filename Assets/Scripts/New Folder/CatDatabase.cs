using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 猫猫数据库
public class CatDatabase : MonoBehaviour
{
    private Dictionary<int, CatBase> allCats = new Dictionary<int, CatBase>();

    public void Initialize()
    {
        // 场景加载时自动注册所有猫猫
        CatBase[] catsInScene = FindObjectsOfType<CatBase>();
        foreach (CatBase cat in catsInScene)
        {
            RegisterCat(cat);
        }
    }

    public void RegisterCat(CatBase cat)
    {
        if (!allCats.ContainsKey(cat.catID))
        {
            allCats.Add(cat.catID, cat);
        }
    }

    public CatBase GetCat(int catID)
    {
        return allCats.ContainsKey(catID) ? allCats[catID] : null;
    }

    public List<CatBase> GetAllCats()
    {
        return new List<CatBase>(allCats.Values);
    }
}
