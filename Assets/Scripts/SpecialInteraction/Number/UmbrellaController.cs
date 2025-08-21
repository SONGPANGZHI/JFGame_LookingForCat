using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbrellaController : MonoBehaviour
{
    public int umbrellaID;              // 雨伞的唯一标识符
    public Sprite openUmbrella;
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
            this.GetComponent<BoxCollider2D>().enabled = false;
            PlayerPrefs.SetString(umbrellaTag + umbrellaID, umbrellaID.ToString());
            Debug.Log(umbrellaTag + umbrellaID);
            UpdateVisuals();
            manager.ReportUmbrellaOpened();
        }
    }

    // 更新雨伞视觉表现
    private void UpdateVisuals()
    {
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.GetComponent<SpriteRenderer>().sprite = openUmbrella;
    }
}
