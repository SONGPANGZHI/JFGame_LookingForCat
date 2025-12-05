using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindLove : MonoBehaviour
{
    public int loveID;

    private ItemType itemType = ItemType.Love;
    private void Awake()
    {
        JuageLoveState();
    }


    /// <summary>
    /// 初始化 
    /// </summary>
    public void Init()
    {
        this.GetComponent<Collider2D>().enabled = true;
    }

    /// <summary>
    /// 点击小爱心
    /// </summary>
    public void ClickLoveObject()
    {
        // 改变小爱心数量显示
        CrabManager.Instance.ClickItem(itemType);

        gameObject.SetActive(false);
        PlayerPrefs.SetString("LoveKey_" + loveID, "Love_" + loveID);
    }

    /// <summary>
    /// 判断小爱心状态
    /// </summary>
    public void JuageLoveState()
    {
        if (PlayerPrefs.HasKey("LoveKey_" + loveID))
        {
            gameObject.SetActive(false);
        }
    }

}
