using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 条件检查器
public class ConditionChecker : MonoBehaviour
{
    public void CheckAllConditions()
    {
        foreach (CatBase cat in GameManager.Instance.catDatabase.GetAllCats())
        {
            if (cat is ConditionalCat conditionalCat)
            {
                conditionalCat.CheckCondition();
            }
        }
    }

    public void CheckConditions()
    {
        // 只检查未找到的条件猫猫
        foreach (CatBase cat in GameManager.Instance.catDatabase.GetAllCats())
        {
            if (!cat.isFound && cat is ConditionalCat conditionalCat)
            {
                conditionalCat.CheckCondition();
            }
        }
    }
}
