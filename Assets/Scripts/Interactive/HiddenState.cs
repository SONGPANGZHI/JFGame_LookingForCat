using UnityEngine;

/// <summary>
/// 互动状态（HiddenState）在互动状态中，物品不是直接可见的，而是隐藏在其他物品下（如点击树叶或箱子打开）。
/// </summary>
public class HiddenState : InteractionState
{
    private ItemClick hiddenItem;  // 被隐藏的物品
    private GameObject interactableObject;  // 可互动的物体（例如树叶、箱子等）

    public HiddenState(ItemClick item, GameObject interactable)
    {
        hiddenItem = item;
        interactableObject = interactable;
    }

    public override void EnterState()
    {
        Debug.Log("进入互动状态");
        // 可以设置互动物体为可点击，或者显示交互提示
        interactableObject.SetActive(true);
    }

    public override void ExitState()
    {
        Debug.Log("退出互动状态");
        // 清理互动物体的状态
        interactableObject.SetActive(false);
    }

    public override void UpdateState()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == interactableObject)
                {
                    interactableObject.SetActive(false);  // 移除互动物体，显示隐藏物品
                    hiddenItem.gameObject.SetActive(true);  // 显示隐藏的物品
                    hiddenItem.Found();  // 标记物品为已找到
                }
            }
        }
    }
}
