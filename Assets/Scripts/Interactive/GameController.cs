using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 设置初始状态,随着物品的发现切换到其他状态。
/// </summary>
public class GameController : MonoBehaviour
{
    public static GameController instance;

    public GameStateManager stateManager;       // 游戏状态管理器
    public SaveManagement saveManager;          //游戏保存管理器

    [SerializeField]
    private List<ItemClick> visibleItem;
    [SerializeField]
    private List<ItemClick> hiddenItem;
    [SerializeField]
    private List<ItemClick> unlockItem;


    public ItemClick catItem;                   //点击物品

    public static bool clickCompleted;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    void Update()
    {
        if (catItem == null) return;

        if (catItem.isFound && !clickCompleted)
        {
            clickCompleted = true;
            JudgeItemState();
        }
    }

    public void JudgeItemState()
    {
        switch (catItem.itemState)
        {
            case ItemState.Visible:
                // 如果可见物品找到，进入互动状态
                InteractionState interactiveState = new VisibleState(catItem);
                stateManager.SwitchState(interactiveState);
                break;
            case ItemState.Hidden:
                //简单交互
                InteractionState hiddenState = new HiddenState(catItem,catItem.hiddenObjects, catItem.interactingObjects);
                stateManager.SwitchState(hiddenState);
                break;
            case ItemState.Locked:
                //复杂
                InteractionState unlockState = new UnlockState(catItem.hiddenObjects, catItem.interactingObjects);
                stateManager.SwitchState(unlockState);
                break;
        }
      
    }
}
