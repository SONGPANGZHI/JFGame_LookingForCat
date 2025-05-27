using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameTypeTag : MonoBehaviour
{
    
}

//public enum InteractionState
//{ 
//    None,
//    NoInteraction,
//    SimpleInteraction,
//    ComplexInteractions,
//}

//物品状态
public enum ItemState
{
    // 基础状态
    Normal,         // 普通状态（无交互）
    Visible,        // 直接可见可点击
    Hidden,         // 需要互动后才可见
    Locked,         // 需要解锁条件

    // 特殊状态
    Disabled,       // 已禁用（不可交互）
    Collected,      // 已收集
    Used            // 已使用（如钥匙开门后）
}

