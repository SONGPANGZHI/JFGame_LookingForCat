using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 条件猫类
public class ConditionalCat : CatBase
{

    public ConditionType conditionType;

    [Header("不同条件配置")]
    public int[] requiredCatIDs;          // 需要找到的猫ID数组
    public int requiredItemCount = 10;    // 需要收集的物品数量
    public GameObject[] hiddenObjects;    // 隐藏的物体

    public GameObject[] defaultHiddenObjects;    // 默认的物体

    private void Start()
    {
        Initialize();

        if (GameManager.Instance.progressManager.IsCatFound(47))
        {
            SetHiddenState(false);
        }
    }

    // 判断条件是否满足
    public void CheckCondition()
    {
        if (isFound) return;
       
        bool conditionMet = false;

        switch (conditionType)
        {
            case ConditionType.FindSpecificCat:         // 找到特定小猫
                conditionMet = AreAllCatsFound();
                break;

            case ConditionType.CollectItems:            // 收集物品
                conditionMet = GameManager.Instance.progressManager.GetItemCount() >= requiredItemCount;
                break;

            case ConditionType.Combination:             // 组合条件
                conditionMet = AreAllCatsFound() &&
                              GameManager.Instance.progressManager.GetItemCount() >= requiredItemCount;
                break;
        }

        if (conditionMet)
        {
            SetHiddenState(false);
        }
    }

    // 检查是否所有需要的猫都被找到了
    private bool AreAllCatsFound()
    {
        if (requiredCatIDs == null || requiredCatIDs.Length == 0)
            return false;

        foreach (int catID in requiredCatIDs)
        {
            if (!GameManager.Instance.progressManager.IsCatFound(catID))
                return false;
        }
        return true;
    }

    private void SetHiddenState(bool hidden)
    {
        foreach (var obj in hiddenObjects)
        {
            obj.SetActive(!hidden);
            this.GetComponent<Collider2D>().enabled = !hidden;
        }

        foreach (var item in defaultHiddenObjects)
        {
            item.SetActive(hidden);
        }
    }

    // 由InputManager检测触摸
    public void OnTapped()
    {
        if (!isFound && !IsHidden())
        {
            OnCatFound();
        }
    }

    public bool IsHidden()
    {
        return hiddenObjects.Length > 0 && !hiddenObjects[0].activeSelf;
    }

}
