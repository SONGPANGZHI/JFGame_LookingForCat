using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

public class SaveManagement : MonoBehaviour
{

    public GameSaveData CurrentData;

    private static string SavePath => Path.Combine(Application.persistentDataPath, "GameSaveData.json");

    private void Awake()
    {
        InitializeData();
    }

    private void InitializeData()
    {
        if (File.Exists(SavePath))
        {
            LoadGameSaveData();
        }
        else
        {
            Debug.Log("无存档文件，创建新数据并保存初始文件");
            CurrentData = new GameSaveData();

            // 立即保存创建初始文件
            SaveData();
        }

    }

    public void LoadGameSaveData()
    {
        try
        {
            if (!File.Exists(SavePath))
            {
                Debug.LogWarning($"存档文件不存在: {SavePath}");
                return;
            }

            string json = File.ReadAllText(SavePath);
            CurrentData = JsonUtility.FromJson<GameSaveData>(json);

            // 确保反序列化后的对象不为null
            if (CurrentData == null)
            {
                Debug.LogWarning("反序列化失败，创建新数据");
                CurrentData = new GameSaveData();
            }

            // 确保所有列表已初始化
            CurrentData.findCatNameKey = CurrentData.findCatNameKey ?? new List<string>();

            Debug.Log("游戏数据加载成功");
        }
        catch (Exception e)
        {
            Debug.LogError($"加载数据失败: {e.Message}");
            CurrentData = new GameSaveData();
        }

    }

    // 保存列表到JSON文件
    public void SaveData()
    {
        try
        {
            string json = JsonUtility.ToJson(CurrentData, true);
            File.WriteAllText(SavePath, json);
            Debug.Log($"游戏数据保存成功: {SavePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"保存数据失败: {e.Message}");
        }
    }

    //保存ItemName
    public void SaveItemName(string key)
    {
        CurrentData.findCatNameKey.Add(key);
        SaveData();
    }

}

//物品状态
public enum ItemState
{
    // 基础状态
    Normal,         // 普通状态（无交互）
    Visible,        // 直接可见可点击
    Hidden,         // 需要互动后才可见
    Locked,         // 需要解锁条件

    // 特殊状态
    Disabled,       // 已禁用（不可交互）
    Collected,      // 已收集
    Used            // 已使用（如钥匙开门后）
}

public class GameSaveData
{ 
    public List<string> findCatNameKey = new List<string>();
}
