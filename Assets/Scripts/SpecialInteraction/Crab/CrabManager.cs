using System.Collections.Generic;
using UnityEngine;

public class CrabManager : MonoBehaviour
{
    public static CrabManager Instance;

    public List<CrabNum> crabNumSprite;

    public SpriteRenderer decadeSprite;             // 十位数精灵
    public SpriteRenderer unitSprite;               // 个位数精灵

    public Collider2D crabCat;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        if(!PlayerPrefs.HasKey("SaveCrabAmount"))
            PlayerPrefs.SetInt("SaveCrabAmount", 20);


    }

    private void Start()
    {
        LoadCrabNumInit();
    }

    /// <summary>
    /// 加载螃蟹数量初始化
    /// </summary>
    public void LoadCrabNumInit()
    {
        UpdateCrabNum(PlayerPrefs.GetInt("SaveCrabAmount"));
    }

    /// <summary>
    /// 更新数字显示
    /// </summary>
    /// <param name="num"></param>
    public void UpdateCrabNum(int num)
    {
        // 每个CrabNum对象的numSprites列表中，第0个是个位数，第1个是十位数
        decadeSprite.sprite = crabNumSprite[num].numSprites[0]; 
        unitSprite.sprite = crabNumSprite[num].numSprites[1];

        if (num == 0)
            OpenCarbCollider();

        PlayerPrefs.SetInt("SaveCrabAmount", num);
    }

    /// <summary>
    /// 打开螃蟹碰撞体
    /// </summary>
    public void OpenCarbCollider()
    {
        crabCat.enabled = true;
    }

    //public static CrabManager instance;

    //// 存储所有已收集的螃蟹ID
    //private HashSet<int> collectedCrabs = new HashSet<int>();
    //private bool isDataLoaded = false;

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        LoadAllCrabData();
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    ///// <summary>
    ///// 加载所有螃蟹数据
    ///// </summary>
    //private void LoadAllCrabData()
    //{
    //    if (isDataLoaded) return;

    //    collectedCrabs.Clear();

    //    // 假设最多有20只螃蟹，可以根据实际情况调整
    //    for (int i = 0; i < 20; i++)
    //    {
    //        string key = GetCrabKey(i);
    //        if (PlayerPrefs.HasKey(key))
    //        {
    //            collectedCrabs.Add(i);
    //        }
    //    }

    //    isDataLoaded = true;
    //    Debug.Log($"螃蟹数据加载完成，已收集 {collectedCrabs.Count} 只螃蟹");
    //}

    ///// <summary>
    ///// 获取螃蟹的存储Key
    ///// </summary>
    //private string GetCrabKey(int crabID)
    //{
    //    return $"CrabKey_{crabID}";
    //}

    ///// <summary>
    ///// 检查螃蟹是否已被收集
    ///// </summary>
    //public bool IsCrabCollected(int crabID)
    //{
    //    return collectedCrabs.Contains(crabID);
    //}

    ///// <summary>
    ///// 收集螃蟹
    ///// </summary>
    //public void CollectCrab(int crabID)
    //{
    //    if (!collectedCrabs.Contains(crabID))
    //    {
    //        collectedCrabs.Add(crabID);
    //        SaveCrabData(crabID);
    //        Debug.Log($"螃蟹 {crabID} 已收集");
    //    }
    //}

    ///// <summary>
    ///// 保存螃蟹数据
    ///// </summary>
    //private void SaveCrabData(int crabID)
    //{
    //    string key = GetCrabKey(crabID);
    //    PlayerPrefs.SetString(key, $"Crab_{crabID}");
    //    PlayerPrefs.Save();
    //}

    ///// <summary>
    ///// 获取已收集的螃蟹数量
    ///// </summary>
    //public int GetCollectedCrabCount()
    //{
    //    return collectedCrabs.Count;
    //}

    ///// <summary>
    ///// 获取所有已收集的螃蟹ID
    ///// </summary>
    //public List<int> GetAllCollectedCrabIDs()
    //{
    //    return new List<int>(collectedCrabs);
    //}

    ///// <summary>
    ///// 重置指定螃蟹的状态
    ///// </summary>
    //public void ResetCrab(int crabID)
    //{
    //    if (collectedCrabs.Contains(crabID))
    //    {
    //        collectedCrabs.Remove(crabID);
    //        string key = GetCrabKey(crabID);
    //        PlayerPrefs.DeleteKey(key);
    //        PlayerPrefs.Save();
    //        Debug.Log($"螃蟹 {crabID} 状态已重置");
    //    }
    //}

    ///// <summary>
    ///// 重置所有螃蟹状态
    ///// </summary>
    //public void ResetAllCrabs()
    //{
    //    collectedCrabs.Clear();

    //    for (int i = 0; i < 20; i++)
    //    {
    //        string key = GetCrabKey(i);
    //        if (PlayerPrefs.HasKey(key))
    //        {
    //            PlayerPrefs.DeleteKey(key);
    //        }
    //    }

    //    PlayerPrefs.Save();
    //    isDataLoaded = false;
    //    Debug.Log("所有螃蟹状态已重置");
    //}

    ///// <summary>
    ///// 重新加载数据（用于调试或数据同步）
    ///// </summary>
    //public void ReloadData()
    //{
    //    isDataLoaded = false;
    //    LoadAllCrabData();
    //}
}

[System.Serializable]
public class CrabNum
{
    public int ID;
    public List<Sprite> numSprites;
}
