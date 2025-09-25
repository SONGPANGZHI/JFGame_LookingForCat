using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static string SAVE_KEY = "CatGameSaveData";

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
                            cat.catAnim.Skeleton.SetColor(Color.gray);
                            cat.PlayAnim(0, "Sports", cat.loopAnim);
                        }
                        
                    }
                    else
                    {
                        cat.GetComponent<SpriteRenderer>().color = Color.gray;
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
    }

}
