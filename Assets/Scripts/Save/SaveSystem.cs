using Spine.Unity;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static string SAVE_KEY = "CatGameSaveData_Steam";
    private const string FIRST_RUN_KEY = "CatGame_FirstRun";
    private const string GAME_VERSION = "1.0.0"; // 游戏版本号

    void Start()
    {
        CheckFirstRunAndClearData();
    }

    // 检查是否是第一次运行，如果是则清理数据
    private void CheckFirstRunAndClearData()
    {
        string savedVersion = PlayerPrefs.GetString("GameVersion", "");
        bool isFirstRun = !PlayerPrefs.HasKey(FIRST_RUN_KEY);

        // 如果是第一次运行或版本更新
        if (isFirstRun || savedVersion != GAME_VERSION)
        {
            Debug.Log("检测到首次运行或版本更新，清理旧数据...");
            ClearAllSaveData();

            // 标记为已运行过
            PlayerPrefs.SetInt(FIRST_RUN_KEY, 1);
            PlayerPrefs.SetString("GameVersion", GAME_VERSION);
            PlayerPrefs.Save();

            Debug.Log("首次运行数据初始化完成");
        }
    }

    // 清理所有存档数据
    public void ClearAllSaveData()
    {
        // 删除主存档
        PlayerPrefs.DeleteKey(SAVE_KEY);

        // 删除其他可能的PlayerPrefs数据（保留设置等）
         PlayerPrefs.DeleteAll(); // 谨慎使用，会删除所有设置

        // 确保保存
        PlayerPrefs.Save();

        Debug.Log("所有存档数据已清除");
    }

    // 保存数据结构
    public void SaveGame()
    {
        SaveData data = new SaveData
        {
            foundCatIDs = GameManager.Instance.progressManager.GetFoundCatIDs(),
            itemCount = GameManager.Instance.progressManager.ItemCount,
            // 可以添加其他需要保存的数据
        };

        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_KEY, jsonData);
        PlayerPrefs.Save();
    }

    // 加载数据
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string jsonData = PlayerPrefs.GetString(SAVE_KEY);
            SaveData data = JsonUtility.FromJson<SaveData>(jsonData);

            Debug.Log(jsonData);

            // 恢复进度
            GameManager.Instance.progressManager.LoadProgress(data.foundCatIDs, data.itemCount);

            Debug.Log(data.foundCatIDs.Length);
            // 恢复猫猫状态
            foreach (int catID in data.foundCatIDs)
            {
                CatBase cat = GameManager.Instance.catDatabase.GetCat(catID);
                if (cat != null)
                {
                    cat.isFound = true;
                    if (cat.GetComponent<SpriteRenderer>() == null)
                    {
                        if (cat.catAnim != null)
                        {
                            cat.catAnim.Skeleton.SetColor(cat.RandomCatColor());
                            cat.PlayAnim(0, "Sports", cat.loopAnim);
                        }

                    }
                    else
                    {
                        cat.GetComponent<SpriteRenderer>().color = cat.RandomCatColor();
                    }

                    cat.SpawnEffect();
                }
            }

            // 检查条件
            GameManager.Instance.conditionChecker.CheckAllConditions();

            // 更新UI
            UIManager.Instance.UpdateProgressUI();

            Debug.Log("游戏进度已加载");
        }
        else
        {
            Debug.Log("没有找到存档数据，开始新游戏");
            // 可以在这里初始化新游戏状态
        }
    }


    //public static string SAVE_KEY = "CatGameSaveData_Steam";

    //// 保存数据结构
    //public void SaveGame()
    //{
    //    SaveData data = new SaveData
    //    {
    //        foundCatIDs = GameManager.Instance.progressManager.GetFoundCatIDs(),
    //        itemCount = GameManager.Instance.progressManager.ItemCount,
    //        // 可以添加其他需要保存的数据
    //    };

    //    string jsonData = JsonUtility.ToJson(data);
    //    PlayerPrefs.SetString(SAVE_KEY, jsonData);
    //    PlayerPrefs.Save();
    //}

    //// 加载数据
    //public void LoadGame()
    //{
    //    if (PlayerPrefs.HasKey(SAVE_KEY))
    //    {
    //        string jsonData = PlayerPrefs.GetString(SAVE_KEY);
    //        SaveData data = JsonUtility.FromJson<SaveData>(jsonData);

    //        Debug.Log(jsonData);

    //        // 恢复进度
    //        GameManager.Instance.progressManager.LoadProgress(data.foundCatIDs, data.itemCount);

    //        Debug.Log(data.foundCatIDs.Length);
    //        // 恢复猫猫状态
    //        foreach (int catID in data.foundCatIDs)
    //        {
    //            CatBase cat = GameManager.Instance.catDatabase.GetCat(catID);
    //            if (cat != null)
    //            {
    //                cat.isFound = true;
    //                if (cat.GetComponent<SpriteRenderer>() == null)
    //                {
    //                    if (cat.catAnim != null)
    //                    {
    //                        cat.catAnim.Skeleton.SetColor(cat.RandomCatColor());
    //                        cat.PlayAnim(0, "Sports", cat.loopAnim);
    //                    }

    //                }
    //                else
    //                {
    //                    cat.GetComponent<SpriteRenderer>().color = cat.RandomCatColor();
    //                }

    //                cat.SpawnEffect();
    //            }
    //        }

    //        // 检查条件
    //        GameManager.Instance.conditionChecker.CheckAllConditions();

    //        // 更新UI
    //        UIManager.Instance.UpdateProgressUI();

    //        Debug.Log("游戏进度已加载");
    //    }
    //}

}
