using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbrellaManager : MonoBehaviour
{
    [Header("ID_096")]
    public int totalUmbrellas;                  // 总雨伞数量
    private int openedUmbrellas;            // 已打开的雨伞数量
    public VisibleCat hiddenCat;                // 显示猫猫
    public bool isCatVisible = false;           // 是否显示猫猫


    private void Start()
    {
        if (!PlayerPrefs.HasKey("UmbrellaKey"))
            openedUmbrellas = 0;
        else
            openedUmbrellas = PlayerPrefs.GetInt("UmbrellaKey");

        bool isCompleted = GameManager.Instance.progressManager.IsCatFound(96);

        if (isCompleted)
        {
            hiddenCat.gameObject.SetActive(true); // 显示目标物体
            hiddenCat.GetComponent<SpriteRenderer>().color = Color.gray;
        }


    }

    // 报告雨伞被打开
    public void ReportUmbrellaOpened()
    {
        openedUmbrellas += 1;
        PlayerPrefs.SetInt("UmbrellaKey", openedUmbrellas);
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
