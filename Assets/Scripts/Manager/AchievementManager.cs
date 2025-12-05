using Steamworks;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }

    private HashSet<string> unlocked = new HashSet<string>();

    private void Awake()
    {
        if(Instance==null)
            Instance = this;
    }

    private void Start()
    {
        LoadUnlockedAchievements();
    }

    public void UnlockAchievement(string achievementId)
    {
        // 已解锁过的成就,不再重复触发
        if (unlocked.Contains(achievementId))
            return;

        // 解锁
        SteamUserStats.SetAchievement(achievementId);
        SteamUserStats.StoreStats();

        unlocked.Add(achievementId);

        Debug.Log($"[AchievementManager] 成就解锁成功：{achievementId}");
    }

    /// <summary>
    /// 从 Steam API 读取所有已完成的成就，加入缓存
    /// </summary>
    private void LoadUnlockedAchievements()
    {
        if (!SteamManager.Initialized)
            return;

        int count = (int)SteamUserStats.GetNumAchievements();
        for (int i = 0; i < count; i++)
        {
            string id = SteamUserStats.GetAchievementName((uint)i);

            bool achieved = false;
            SteamUserStats.GetAchievement(id, out achieved);

            if (achieved)
            {
                unlocked.Add(id);
                Debug.Log($"[AchievementManager] 已加载成就：{id}");
            }
        }
    }
}
