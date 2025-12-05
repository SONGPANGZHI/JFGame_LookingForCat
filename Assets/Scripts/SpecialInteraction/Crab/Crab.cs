using UnityEngine;

public class Crab : MonoBehaviour
{
    public int crabID;
    private ItemType itemType = ItemType.Crab;
    public int reduceAmount = 1;

    private void Start()
    {
        JudgeCrabState();
    }

    /// <summary>点击螃蟹</summary>
    public void ClickCrab()
    {
        CrabManager.Instance.ClickItem(itemType);
        gameObject.SetActive(false); // 点击后隐藏

        //int current = PlayerPrefs.GetInt("SaveCrabAmount");

        //if (current <= 0) return;

        //gameObject.SetActive(false);

        // 记录已点击状态
        PlayerPrefs.SetString("CrabKey_" + crabID, "Crab_" + crabID);

        //// 更新数量
        //CrabManager.Instance.UpdateItemAmount(itemType, current - 1);

        //Debug.Log("Crab left = " + (current - 1));
    }

    /// <summary>判断初始化状态</summary>
    public void JudgeCrabState()
    {
        if (PlayerPrefs.HasKey("CrabKey_" + crabID))
        {
            if (crabID < 6)
                transform.parent.GetComponent<CrabDisplacement>().CloseCrab();
            else
                gameObject.SetActive(false);
        }
        else
        {
            if (crabID < 6)
                transform.parent.GetComponent<Collider2D>().enabled = true;
            else
                gameObject.SetActive(true);
        }
    }

}
