using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    [Header("基础信息")]
    public int itemID;          // 唯一标识符


    [Header("物品属性")]
    public bool isConsumable = false; // 是否可消耗
    public int maxStack = 1;      // 最大堆叠数
    public int stackCount = 1;    // 当前堆叠数

    [Header("使用效果")]
    public AudioClip useSound;    // 使用音效
    public ParticleSystem useEffect; // 使用特效

  
}
