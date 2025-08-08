
// 类型
public enum CatType
{
    Visible,            // 直接可见的猫猫
    Interactive,        // 需要简单交互的猫猫
    Special             // 需要特殊条件触发的猫猫
}

// 条件类型
public enum ConditionType
{
    FindSpecificCat,  // 需要找到特定的猫
    Combination,      // 需要组合条件（如找到另一只猫和收集物品）
    FindOtherCat,    // 需要找到另一只猫
    CollectItems,    // 需要收集物品
}

// 保存数据结构
[System.Serializable]
public class SaveData
{
    public int[] foundCatIDs;
    public int itemCount;
}

