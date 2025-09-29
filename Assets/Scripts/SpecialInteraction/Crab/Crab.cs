using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    public int crabID;

    private void Awake()
    {
        JuageCrabState();
    }


    /// <summary>
    /// 初始化 
    /// </summary>
    public void Init()
    {
        this.GetComponent<Collider2D>().enabled = true;
    }

    /// <summary>
    /// 点击螃蟹
    /// </summary>
    public void ClickCrab()
    { 
        gameObject.SetActive(false);
        PlayerPrefs.SetString("CrabKey_" + crabID, "Crab_"+ crabID);
    }

    /// <summary>
    /// 判断螃蟹状态
    /// </summary>
    public void JuageCrabState()
    {
        if (PlayerPrefs.HasKey("CrabKey_" + crabID))
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
 
}
