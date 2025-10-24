using System.Collections.Generic;
using UnityEngine;

public class CrabManager : MonoBehaviour
{
    public static CrabManager Instance;

    public List<CollectibleItem> collectibleItems; // 所有可收集物品列表

    [Header("螃蟹相关")]
    public Collider2D crabCat;
    public Collider2D loveCat;
    public Collider2D catID_127;

    public Collider2D earthwormCat;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (!PlayerPrefs.HasKey("SaveCrabAmount"))
            PlayerPrefs.SetInt("SaveCrabAmount", 20);

        if (!PlayerPrefs.HasKey("SaveEarthwormAmount"))
            PlayerPrefs.SetInt("SaveEarthwormAmount", 5);

        if (!PlayerPrefs.HasKey("SaveLoveAmount"))
            PlayerPrefs.SetInt("SaveLoveAmount", 10);

    }
    private void Start()
    {
        LoadAllItems();
    }

    /// <summary>
    /// 加载所有物品的显示
    /// </summary>
    public void LoadAllItems()
    {
        foreach (var item in collectibleItems)
        {
            LoadItemDisplay(item);
        }
    }

    /// <summary>
    /// 加载单个物品的显示
    /// </summary>
    /// <param name="item">物品</param>
    private void LoadItemDisplay(CollectibleItem item)
    {
        int currentAmount = PlayerPrefs.GetInt(item.saveKey, item.initialAmount);
        UpdateItemDisplay(item, currentAmount);
        Debug.Log($"加载 {item.itemName}: {currentAmount}");
    }

    /// <summary>
    /// 更新物品显示
    /// </summary>
    /// <param name="item">物品</param>
    /// <param name="amount">数量</param>
    public void UpdateItemDisplay(CollectibleItem item, int amount)
    {
        // 更新UI显示
        if (item.decadeSprite != null && item.unitSprite != null && item.numberSprites != null)
        {
            UpdateNumberSprites(item, amount);
        }

        // 检查是否达到目标数量
        if (amount <= item.targetAmount)
        {
            OnItemTargetReached(item);
        }
    }

    /// <summary>
    /// 更新指定物品的数量
    /// </summary>
    /// <param name="itemType">物品类型</param>
    /// <param name="amount">数量</param>
    public void UpdateItemAmount(ItemType itemType, int amount)
    {
        CollectibleItem item = GetItemByType(itemType);
        if (item != null)
        {
            // 确保数量在有效范围内
            amount = Mathf.Clamp(amount, 0, item.maxAmount);

            // 更新UI显示
            if (item.decadeSprite != null && item.unitSprite != null)
            {
                UpdateNumberSprites(item, amount);
            }

            // 保存数据
            PlayerPrefs.SetInt(item.saveKey, amount);

            // 检查是否达到目标数量
            if (amount <= item.targetAmount)
            {
                OnItemTargetReached(item);
            }

        }
    }


    /// <summary>
    /// 更新数字精灵显示
    /// </summary>
    /// <param name="item">物品</param>
    /// <param name="amount">数量</param>
    public void UpdateNumberSprites(CollectibleItem item, int amount)
    {
        // 计算十位和个位
        int decade = amount / 10;
        int unit = amount % 10;

        // 更新精灵显示
        if (item.numberSprites != null && item.numberSprites.Count > 0)
        {
            // 假设numberSprites列表中包含0-9的数字精灵
            if (decade < item.numberSprites.Count)
                item.decadeSprite.sprite = item.numberSprites[decade];

            if (unit < item.numberSprites.Count)
                item.unitSprite.sprite = item.numberSprites[unit];
        }
    }

    /// <summary>
    /// 当物品达到目标数量时的处理
    /// </summary>
    /// <param name="item">物品</param>
    private void OnItemTargetReached(CollectibleItem item)
    {
        Debug.Log($"{item.itemName} 已达到目标数量 {item.targetAmount}!");

        // 触发相应的事件
        switch (item.itemType)
        {
            case ItemType.Crab:
                // 螃蟹特殊逻辑
                OpenCarbCollider();
                break;
            case ItemType.Earthworm:
                // 蚯蚓达到目标数量的逻辑
                OpenEarthwormCollider();
                break;
            case ItemType.Love:
                // 小爱心达到目标数量的逻辑
                OpenLoveCollider();
                break;
        }

    }

    /// <summary>
    /// 根据类型获取物品
    /// </summary>
    /// <param name="itemType">物品类型</param>
    /// <returns>物品对象</returns>
    private CollectibleItem GetItemByType(ItemType itemType)
    {
        return collectibleItems.Find(item => item.itemType == itemType);
    }

    /// <summary>
    /// 打开螃蟹碰撞体
    /// </summary>
    public void OpenCarbCollider()
    {
        if (crabCat != null)
        {
            crabCat.transform.GetChild(0).gameObject.SetActive(false);
            crabCat.enabled = true;
        }
    }

    public void OpenLoveCollider()
    {
        if (loveCat != null)
        {
            loveCat.transform.GetChild(0).gameObject.SetActive(false);
            catID_127.GetComponent<SpriteRenderer>().enabled = true;
            catID_127.enabled = true;
            loveCat.enabled = true;
        }
    }

    public void OpenEarthwormCollider()
    {
        if (earthwormCat != null)
        {
            earthwormCat.transform.GetChild(0).gameObject.SetActive(false);
            earthwormCat.enabled = true;
        }
    }

    /// <summary>
    /// 重置所有物品数量
    /// </summary>
    public void ResetAllItems()
    {
        foreach (var item in collectibleItems)
        {
            PlayerPrefs.SetInt(item.saveKey, item.initialAmount);
            UpdateItemAmount(item.itemType, item.initialAmount);
        }
    }

    /// <summary>
    /// 重置指定物品数量
    /// </summary>
    /// <param name="itemType">物品类型</param>
    public void ResetItem(ItemType itemType)
    {
        CollectibleItem item = GetItemByType(itemType);
        if (item != null)
        {
            PlayerPrefs.SetInt(item.saveKey, item.initialAmount);
            UpdateItemAmount(itemType, item.initialAmount);
        }
    }
}


[System.Serializable]
public class CollectibleItem
{
    [Header("基础设置")]
    public ItemType itemType;           // 物品类型
    public string itemName;             // 物品名称
    public string saveKey;              // 保存键值

    [Header("数量设置")]
    public int initialAmount = 0;       // 初始数量
    public int targetAmount = 10;       // 目标数量
    public int maxAmount = 99;          // 最大数量

    [Header("UI显示")]
    public SpriteRenderer decadeSprite; // 十位数精灵
    public SpriteRenderer unitSprite;   // 个位数精灵
    public List<Sprite> numberSprites;  // 数字精灵列表 (0-9)

}

/// <summary>
/// 物品类型枚举
/// </summary>
public enum ItemType
{
    Crab,       // 螃蟹
    Earthworm,  // 香蕉
    Love,       // 小爱心

    // 可以继续添加其他类型
}