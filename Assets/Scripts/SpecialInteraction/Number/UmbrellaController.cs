using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbrellaController : MonoBehaviour
{
    public int umbrellaID;              // 雨伞的唯一标识符
    public bool isOpen = false;         // 雨伞当前状态

    // 引用全局管理器
    private UmbrellaManager manager;
    private const string umbrellaTag = "Umbrella"; // 雨伞的标签

    void Start()
    {
        manager = FindObjectOfType<UmbrellaManager>();
        LoadUmberlla();
    }

    public void LoadUmberlla()
    {
        if (PlayerPrefs.HasKey(umbrellaTag + umbrellaID))
        {
            isOpen = true;
            UpdateVisuals();
        }
    }

    // 点击雨伞时调用
    public void UmbrellaClickEvent()
    {
        if (!isOpen)
        {
            isOpen = true;
            PlayerPrefs.SetString(umbrellaTag + umbrellaID, umbrellaID.ToString());
            Debug.Log(umbrellaTag + umbrellaID);
            UpdateVisuals();
            manager.ReportUmbrellaOpened();
        }
    }

    // 更新雨伞视觉表现
    private void UpdateVisuals()
    {
        // 根据雨伞ID更换图片 
        //if(umbrellaID == 0)
        //    this.GetComponent<SpriteRenderer>().sprite = manager.openUmbrella_0; // 更换图片
        //else
        //    this.GetComponent<SpriteRenderer>().sprite = manager.openUmbrella_1; // 更换图片

        this.GetComponent<SpriteRenderer>().color = Color.yellow; // 更改颜色为黄色表示已打开
    }
}
