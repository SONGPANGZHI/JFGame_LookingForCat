using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindEarthworm : MonoBehaviour
{
    public int earthwormID;

    private ItemType itemType = ItemType.Earthworm;
    private void Awake()
    {
        JuageEarthwormState();
    }


    /// <summary>
    /// 初始化 
    /// </summary>
    public void Init()
    {
        this.GetComponent<Collider2D>().enabled = true;
    }

    /// <summary>
    /// 点击蚯蚓
    /// </summary>
    public void ClickEarthwormObject()
    {
        // 改变小爱心数量显示
        CrabManager.Instance.ClickItem(itemType);

        gameObject.SetActive(false);
        PlayerPrefs.SetString("EarthwormKey_" + earthwormID, "Earthworm_" + earthwormID);
    }

    /// <summary>
    /// 判断蚯蚓状态
    /// </summary>
    public void JuageEarthwormState()
    {
        if (PlayerPrefs.HasKey("EarthwormKey_" + earthwormID))
        {
            gameObject.SetActive(false);
        }
    }
}
