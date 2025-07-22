using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 条件猫类
public class ConditionalCat : CatBase
{
    public ConditionType conditionType;

    [Header("不同条件配置")]
    public int requiredCatID;            // 需要找到的猫ID
    public int requiredItemCount = 10;   // 需要收集的物品数量
    public GameObject[] hiddenObjects;   // 隐藏的物体

    private void Start()
    {
        Initialize();
        SetHiddenState(true);
    }

    public void CheckCondition()
    {
        if (isFound) return;

        bool conditionMet = false;

        switch (conditionType)
        {
            case ConditionType.FindSpecificCat:
                conditionMet = GameManager.Instance.progressManager.IsCatFound(requiredCatID);
                break;

            case ConditionType.CollectItems:
                conditionMet = GameManager.Instance.progressManager.GetItemCount() >= requiredItemCount;
                break;

            case ConditionType.Combination:
                conditionMet = GameManager.Instance.progressManager.IsCatFound(requiredCatID) &&
                              GameManager.Instance.progressManager.GetItemCount() >= requiredItemCount;
                break;
        }

        if (conditionMet)
        {
            SetHiddenState(false);
        }
    }

    private void SetHiddenState(bool hidden)
    {
        foreach (var obj in hiddenObjects)
        {
            obj.SetActive(!hidden);
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
