using System.Collections.Generic;
using UnityEngine;

public class CrabManager : MonoBehaviour
{
    public static CrabManager Instance;
    public List<CollectibleItem> collectibleItems;

    [Header("碰撞体")]
    public Collider2D crabCat;
    public Collider2D loveCat;
    public Collider2D catID_127;
    public Collider2D earthwormCat;

    public Sprite loveCat_Sprite;

    public ProgressManager pm;
    public SaveSystem saveSystem;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        LoadAllItems();
    }

    #region 初始化和显示
    public void LoadAllItems()
    {
        foreach (var item in collectibleItems)
        {
            int amount = pm.GetItem(item.itemType);
            UpdateItemDisplay(item, amount);
        }
    }

    private void UpdateItemDisplay(CollectibleItem item, int amount)
    {
        UpdateNumberSprites(item, amount);

        if (amount <= item.targetAmount)
            OnItemTargetReached(item);
    }

    private void UpdateNumberSprites(CollectibleItem item, int amount)
    {
        if (item.numberSprites == null || item.numberSprites.Count == 0) return;

        int decade = amount / 10;
        int unit = amount % 10;

        if (item.decadeSprite != null && decade < item.numberSprites.Count)
            item.decadeSprite.sprite = item.numberSprites[decade];

        if (item.unitSprite != null && unit < item.numberSprites.Count)
            item.unitSprite.sprite = item.numberSprites[unit];
    }
    #endregion

    #region 点击逻辑
    public void ClickItem(ItemType type)
    {
        int current = pm.GetItem(type);
        current = Mathf.Max(0, current - 1);
        pm.UpdateItem(type, current);

        var item = collectibleItems.Find(i => i.itemType == type);
        if (item != null)
            UpdateItemDisplay(item, current);

 
    }
    #endregion

    #region 碰撞体控制
    private void OnItemTargetReached(CollectibleItem item)
    {
        switch (item.itemType)
        {
            case ItemType.Crab: OpenCrabCollider(); break;
            case ItemType.Earthworm: OpenEarthwormCollider(); break;
            case ItemType.Love: OpenLoveCollider(); break;
        }
    }

    private void OpenCrabCollider()
    {
        if (!crabCat) return;
        crabCat.transform.GetChild(0).gameObject.SetActive(false);
        crabCat.enabled = true;
    }

    private void OpenLoveCollider()
    {
        if (!loveCat) return;
        loveCat.transform.GetChild(0).gameObject.SetActive(false);
        catID_127.GetComponent<SpriteRenderer>().enabled = true;
        catID_127.enabled = true;

        loveCat.GetComponent<SpriteRenderer>().sprite = loveCat_Sprite;
        loveCat.enabled = true;
    }

    private void OpenEarthwormCollider()
    {
        if (!earthwormCat) return;
        earthwormCat.transform.GetChild(0).gameObject.SetActive(false);
        earthwormCat.enabled = true;
    }
    #endregion

}


[System.Serializable]
public class CollectibleItem
{
    public ItemType itemType;
    public string itemName;
    public int targetAmount = 0;

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