using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 物品类应该包含物品的基础数据（如是否被找到）和一些常见的行为（如物品被点击时的逻辑）。
/// </summary>
public class ItemClick : MonoBehaviour, IPointerDownHandler
{
    public string itemName;                 // 物品名称
    public bool isFound = false;            // 是否找到
    public GameObject associatedObject;     // 物品相关的物体，可能是一个解锁区域或其他对象
    public AudioClip foundSound;            // 找到物品时播放的音效（可选）
    private AudioSource audioSource;

    // 点击事件处理
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isFound)
        {
            Found();
        }
    }

    // 这里可以添加更多的操作，例如更新UI、触发动画、解锁关卡等
    public void Found()
    {
        if (isFound) return;  // 如果物品已被找到，则不再执行

        isFound = true;
        Debug.Log(itemName + " 被找到！");

        // 触发一些操作：比如播放音效
        if (foundSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(foundSound);
        }

        // 如果物品关联了某个物体，解锁它
        if (associatedObject != null)
        {
            associatedObject.SetActive(true);  // 激活/解锁关联的物体
            Debug.Log(itemName + " 解锁了一个新的区域！");
        }

       
    }
}
