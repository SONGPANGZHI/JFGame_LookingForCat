using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbrellaManager : MonoBehaviour
{
    [Header("ID_096")]
    public Sprite openUmbrella_0;               // 打开雨伞的图片
    public Sprite openUmbrella_1;               // 打开雨伞的图片
    public int totalUmbrellas;                  // 总雨伞数量
    private int openedUmbrellas = 0;            // 已打开的雨伞数量
    public VisibleCat hiddenCat;                // 显示猫猫
    public bool isCatVisible = false;           // 是否显示猫猫



   
    // 报告雨伞被打开
    public void ReportUmbrellaOpened()
    {
        openedUmbrellas += 1;
        CheckAllUmbrellasOpened();
    }

    // 检查是否所有雨伞都已打开
    private void CheckAllUmbrellasOpened()
    {
        if (openedUmbrellas >= totalUmbrellas)
        {
            hiddenCat.gameObject.SetActive(true); // 显示目标物体
        }
    }

}
