using Spine.Unity;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private static string saveFileName = "save.dat";
    private static string secretKey;

    private static string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

    // AES Key & IV（16 字节）
    private static byte[] aesKey => Encoding.UTF8.GetBytes("1234567890123456");
    private static byte[] aesIV => Encoding.UTF8.GetBytes("abcdef9876543210");

    #region 密钥和版本检查

    // 懒加载密钥
    private static string GetSecretKey()
    {
        if (string.IsNullOrEmpty(secretKey))
        {
            secretKey = "CatGame_" + SystemInfo.deviceUniqueIdentifier;
            Debug.Log($"[SaveSystem] 初始化密钥: {secretKey}");
        }
        return secretKey;
    }

    // 版本检查
    public void JudgeVersionNumber()
    {
        GetSecretKey();
        string currentVersion = Application.version;
        string savedVersion = PlayerPrefs.GetString("GameVersion", "");

        if (savedVersion != currentVersion)
        {
            // 删除旧存档
            if (File.Exists(SavePath)) File.Delete(SavePath);
            PlayerPrefs.DeleteAll();

            PlayerPrefs.SetString("GameVersion", currentVersion);
            PlayerPrefs.Save();

            Debug.Log($"[SaveSystem] 版本变化 {savedVersion} -> {currentVersion}，已清理旧存档");
        }
    }

    #endregion

    #region 保存游戏

    // 保存游戏数据
    public void SaveGame()
    {
        SaveData data = new SaveData
        {
            foundCatIDs = GameManager.Instance.progressManager.GetFoundCatIDs(),
            itemCount = GameManager.Instance.progressManager.ItemCount,
        };

        SaveDataToFile(data);
        Debug.Log("[SaveSystem] 游戏进度已保存（AES + MD5 文件）");
    }

    // 自动保存默认存档（第一次进入）
    private void SaveDefaultGame()
    {
        SaveData data = new SaveData
        {
            foundCatIDs = new int[0],
            itemCount = 0
        };

        SaveDataToFile(data);
        Debug.Log("[SaveSystem] 默认存档已生成（AES + MD5 文件）");
    }

    // 核心文件保存方法
    private void SaveDataToFile(SaveData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonData);

        // AES 加密
        byte[] encryptedBytes = EncryptAES(jsonBytes, aesKey, aesIV);

        // MD5 校验
        string md5Sig = GetMD5(encryptedBytes);

        // 写入文件：前 32 字节 MD5 + 加密数据
        using (FileStream fs = new FileStream(SavePath, FileMode.Create))
        {
            byte[] md5Bytes = Encoding.UTF8.GetBytes(md5Sig);
            fs.Write(md5Bytes, 0, md5Bytes.Length);
            fs.Write(encryptedBytes, 0, encryptedBytes.Length);
        }
    }

    #endregion

    #region 加载游戏

    public void LoadGame()
    {
        GetSecretKey();

        SaveData data = null;

        if (!File.Exists(SavePath))
        {
            Debug.Log("[SaveSystem] 没有找到存档文件，生成默认存档");
            SaveDefaultGame();
        }

        // 读取文件
        data = LoadGameFromFile();
        if (data == null)
        {
            Debug.LogWarning("[SaveSystem] 存档加载失败，使用默认存档");
            SaveDefaultGame();
            data = LoadGameFromFile();
        }

        // 恢复游戏进度
        GameManager.Instance.progressManager.LoadProgress(data.foundCatIDs, data.itemCount);

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

        GameManager.Instance.conditionChecker.CheckAllConditions();
        UIManager.Instance.UpdateProgressUI();

        Debug.Log("[SaveSystem] 游戏进度加载完成");
    }

    private SaveData LoadGameFromFile()
    {
        if (!File.Exists(SavePath)) return null;

        byte[] allBytes = File.ReadAllBytes(SavePath);
        if (allBytes.Length < 32) return null; // 文件损坏

        byte[] md5Bytes = new byte[32];
        Array.Copy(allBytes, 0, md5Bytes, 0, 32);
        byte[] encryptedBytes = new byte[allBytes.Length - 32];
        Array.Copy(allBytes, 32, encryptedBytes, 0, encryptedBytes.Length);

        string savedMd5 = Encoding.UTF8.GetString(md5Bytes);
        string calcMd5 = GetMD5(encryptedBytes);

        if (savedMd5 != calcMd5)
        {
            Debug.LogWarning("[SaveSystem] 存档被篡改！");
            return null;
        }

        byte[] jsonBytes = DecryptAES(encryptedBytes, aesKey, aesIV);
        string jsonData = Encoding.UTF8.GetString(jsonBytes);
        return JsonUtility.FromJson<SaveData>(jsonData);
    }

    #endregion

    #region AES + MD5 工具

    private static byte[] EncryptAES(byte[] data, byte[] key, byte[] iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            using (var encryptor = aes.CreateEncryptor())
            {
                return encryptor.TransformFinalBlock(data, 0, data.Length);
            }
        }
    }

    private static byte[] DecryptAES(byte[] data, byte[] key, byte[] iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            using (var decryptor = aes.CreateDecryptor())
            {
                return decryptor.TransformFinalBlock(data, 0, data.Length);
            }
        }
    }

    private static string GetMD5(byte[] data)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] hashBytes = md5.ComputeHash(data);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }

    #endregion

}
