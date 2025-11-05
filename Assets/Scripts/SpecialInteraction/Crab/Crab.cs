using UnityEngine;

public class Crab : MonoBehaviour
{
    public int crabID;

    private ItemType itemType = ItemType.Crab;
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
        int currentCrabNum = PlayerPrefs.GetInt("SaveCrabAmount");
        gameObject.SetActive(false);
        PlayerPrefs.SetString("CrabKey_" + crabID, "Crab_"+ crabID);
        // 改变螃蟹数量显示
        CrabManager.Instance.UpdateItemAmount(itemType, currentCrabNum - 1);
        PlayerPrefs.SetInt("SaveCrabAmount", currentCrabNum - 1);
    }

    /// <summary>
    /// 判断螃蟹状态
    /// </summary>
    public void JuageCrabState()
    {
        if (PlayerPrefs.HasKey("CrabKey_" + crabID))
        {
            if (crabID < 6)
            {
                this.transform.parent.GetComponent<CrabDisplacement>().CloseCrab();
            }
            else
                gameObject.SetActive(false);
        }
        else
        {
            if (crabID < 6)
            {
                this.transform.parent.GetComponent<Collider2D>().enabled = true;
            }
            else
                gameObject.SetActive(true);
        }
    }
 
}
