using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 枚举类型
/// </summary>
public enum CatType
{
    Visible,            // 直接可见的猫猫
    Interactive,        // 需要简单交互的猫猫
    Special             // 需要特殊条件触发的猫猫
}

/// <summary>
/// 交互模式枚举
/// </summary>
public enum InteractionMode
{
    None,
    ReplaceSprite,   // 替换精灵
    EnableCollider,  // 启用碰撞体
    Both             // 两者都执行
}

/// <summary>
/// 交互物体动画类型
/// </summary>
public enum InteractiveObjectAnimationType
{
    None,               // 无动画
    PositionMove,       // 位置移动
    CatPosMove,         // 猫猫位置动画
    CatAndObstacleMove, // 猫猫和交互物一起移动
    CustomAnimation,    // 自定义动画
    Both                // 同时有移动和动画

}

/// <summary>
/// 条件类型
/// </summary>
public enum ConditionType
{
    FindSpecificCat,  // 需要找到特定的猫
    Combination,      // 需要组合条件（如找到另一只猫和收集物品）
    FindOtherCat,    // 需要找到另一只猫
    CollectItems,    // 需要收集物品
}

/// <summary>
/// 保存数据结构
/// </summary>
[System.Serializable]
public class SaveData
{
    public int[] foundCatIDs;
    public int itemCount;
}

[System.Serializable]

public class PuzzleSet
{
    public int puzzleID;
    public bool isCompleted;
    public List<GameObject> catOBJ; // 对应的猫猫对象
    public AssemblePuzzlePiece[] pieces;

    [HideInInspector]
    public int assembledCount;
}


[System.Serializable]

public class NumderType
{
    public int IDCat;
    public int itemCount;       // 收集物品数量
    public int itemAllCount;    // 物品总数量
    public VisibleCat catOBJ;   // 对应的猫猫对象

}
