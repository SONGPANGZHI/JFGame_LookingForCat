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
        int currentLoveNum = PlayerPrefs.GetInt("SaveEarthwormIDAmount");
        gameObject.SetActive(false);
        PlayerPrefs.SetString("EarthwormKey_" + earthwormID, "Earthworm_" + earthwormID);

        // 改变小爱心数量显示
        CrabManager.Instance.UpdateItemAmount(itemType, currentLoveNum - 1);
        PlayerPrefs.SetInt("SaveEarthwormIDAmount", currentLoveNum - 1);
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
