using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabManager : MonoBehaviour
{
    public static CrabManager instance;
    

    // 存储所有已收集的螃蟹ID
    private HashSet<int> collectedCrabs = new HashSet<int>();
    private bool isDataLoaded = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            LoadAllCrabData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 加载所有螃蟹数据
    /// </summary>
    private void LoadAllCrabData()
    {
        if (isDataLoaded) return;

        collectedCrabs.Clear();

        // 假设最多有20只螃蟹，可以根据实际情况调整
        for (int i = 0; i < 20; i++)
        {
            string key = GetCrabKey(i);
            if (PlayerPrefs.HasKey(key))
            {
                collectedCrabs.Add(i);
            }
        }

        isDataLoaded = true;
        Debug.Log($"螃蟹数据加载完成，已收集 {collectedCrabs.Count} 只螃蟹");
    }

    /// <summary>
    /// 获取螃蟹的存储Key
    /// </summary>
    private string GetCrabKey(int crabID)
    {
        return $"CrabKey_{crabID}";
    }

    /// <summary>
    /// 检查螃蟹是否已被收集
    /// </summary>
    public bool IsCrabCollected(int crabID)
    {
        return collectedCrabs.Contains(crabID);
    }

    /// <summary>
    /// 收集螃蟹
    /// </summary>
    public void CollectCrab(int crabID)
    {
        if (!collectedCrabs.Contains(crabID))
        {
            collectedCrabs.Add(crabID);
            SaveCrabData(crabID);
            Debug.Log($"螃蟹 {crabID} 已收集");
        }
    }

    /// <summary>
    /// 保存螃蟹数据
    /// </summary>
    private void SaveCrabData(int crabID)
    {
        string key = GetCrabKey(crabID);
        PlayerPrefs.SetString(key, $"Crab_{crabID}");
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 获取已收集的螃蟹数量
    /// </summary>
    public int GetCollectedCrabCount()
    {
        return collectedCrabs.Count;
    }

    /// <summary>
    /// 获取所有已收集的螃蟹ID
    /// </summary>
    public List<int> GetAllCollectedCrabIDs()
    {
        return new List<int>(collectedCrabs);
    }

    /// <summary>
    /// 重置指定螃蟹的状态
    /// </summary>
    public void ResetCrab(int crabID)
    {
        if (collectedCrabs.Contains(crabID))
        {
            collectedCrabs.Remove(crabID);
            string key = GetCrabKey(crabID);
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
            Debug.Log($"螃蟹 {crabID} 状态已重置");
        }
    }

    /// <summary>
    /// 重置所有螃蟹状态
    /// </summary>
    public void ResetAllCrabs()
    {
        collectedCrabs.Clear();

        for (int i = 0; i < 20; i++)
        {
            string key = GetCrabKey(i);
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
            }
        }

        PlayerPrefs.Save();
        isDataLoaded = false;
        Debug.Log("所有螃蟹状态已重置");
    }

    /// <summary>
    /// 重新加载数据（用于调试或数据同步）
    /// </summary>
    public void ReloadData()
    {
        isDataLoaded = false;
        LoadAllCrabData();
    }
}
