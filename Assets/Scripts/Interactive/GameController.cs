using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 设置初始状态,随着物品的发现切换到其他状态。
/// </summary>
public class GameController : MonoBehaviour
{
    public GameStateManager stateManager;  // 游戏状态管理器
    public ItemClick visibleItem;              // 可见物品
    public ItemClick hiddenItem;               // 隐藏物品
    public GameObject interactableObject; // 可互动物体
    public GameObject unlockableArea;     // 解锁区域

    void Start()
    {
        // 初始化状态
        InteractionState visibleState = new VisibleState(visibleItem);
        stateManager.SwitchState(visibleState);  // 设置初始状态为可见状态
    }

    void Update()
    {
        // 游戏进程中根据条件切换状态
        if (visibleItem.isFound)
        {
            // 如果可见物品找到，进入互动状态
            InteractionState interactiveState = new VisibleState(visibleItem);
            stateManager.SwitchState(interactiveState);
        }

        if (hiddenItem.isFound)
        {
            // 如果隐藏物品找到，进入解锁状态
            InteractionState unlockState = new HiddenState(hiddenItem, hiddenItem.associatedObject);
            stateManager.SwitchState(unlockState);
        }
    }
}
