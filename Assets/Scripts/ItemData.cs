using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Visible Objects/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemID;           // 物品唯一标识
    public string displayName;      // 显示名称
    public ItemState itemState;
    public GameObject prefab;       // 物品预制体（可选）
    public bool isFound;            // 是否已找到 - 这个值会被保存

    [Header("简单")]
    public GameObject hiddenObjects;

}
