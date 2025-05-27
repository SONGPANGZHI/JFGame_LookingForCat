using UnityEngine;

/// <summary>
/// 可见状态（VisibleState）当物品处于可见状态时，玩家可以直接点击物品。
/// </summary>
public class VisibleState : InteractionState
{
    private ItemClick visibleItem;  // 可见物品的引用

    public VisibleState(ItemClick item)
    {
        visibleItem = item;
    }

    public override void EnterState()
    {
        Debug.Log("进入可见状态");
        // 启动物品的点击监听，可以在UI上显示提示，物品可以被点击
        visibleItem.gameObject.SetActive(true);
    }

    public override void ExitState()
    {
        Debug.Log("退出可见状态");
        // 如果物品不再可见，可以隐藏物品或清理其他状态
        visibleItem.gameObject.SetActive(false);
    }

    public override void UpdateState()
    {
        // 如果物品被点击，执行物品找到后的逻辑
        if (Input.GetMouseButtonDown(0))  // 监听鼠标点击
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == visibleItem.gameObject)
                {
                    visibleItem.Found();  // 找到物品，执行相关操作
                }
            }
        }
    }
}
