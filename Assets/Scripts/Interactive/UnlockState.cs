using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 解锁状态（UnlockState）在解锁状态下，玩家需要找到特定的物品才能解锁某个区域或物品。
/// </summary>
public class UnlockState: InteractionState
{
    private ItemClick requiredItem;  // 需要的解锁物品
    private GameObject unlockableArea;  // 解锁的区域（例如门、箱子等）

    public UnlockState(ItemClick item, GameObject area)
    {
        requiredItem = item;
        unlockableArea = area;
    }

    public override void EnterState()
    {
        Debug.Log("进入解锁状态");
        unlockableArea.SetActive(false);  // 初始时隐藏解锁区域
    }

    public override void ExitState()
    {
        Debug.Log("退出解锁状态");
    }

    public override void UpdateState()
    {
        if (requiredItem.isFound)  // 如果玩家已经找到了解锁物品
        {
            unlockableArea.SetActive(true);  // 显示解锁区域
            Debug.Log("区域已解锁！");
        }
    }
}
