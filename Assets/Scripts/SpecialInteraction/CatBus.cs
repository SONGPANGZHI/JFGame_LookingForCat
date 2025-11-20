using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ID43、42、41小猫 巴士猫猫
/// </summary>
public class CatBus : MonoBehaviour
{
    public static CatBus Instance;

    [Header("ID_43、42、41")]
    public UniversalMovementController universalMovement;
    public Transform targetPos;     //目标位置
    public Transform moveOBJ;

    public GameObject CAT_42;
    public GameObject CAT_41;
    public GameObject CAT_133;


    [Header("ID_53")]
    public GameObject carClose;
    public GameObject carOpen;
    public GameObject catCat;
    public SpriteRenderer cat_53;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        bool isCompleted = GameManager.Instance.progressManager.IsCatFound(43);
        bool isCompleted_53 = GameManager.Instance.progressManager.IsCatFound(53);
        if (isCompleted)
        {
            moveOBJ.position = targetPos.position;
            CAT_133.GetComponent<SpriteRenderer>().enabled = true;
            CAT_133.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            CAT_42.GetComponent<BoxCollider2D>().enabled = false;
            CAT_41.GetComponent<BoxCollider2D>().enabled = false;

        }
        if(isCompleted_53)
            JuageCat_53();
    }


    public void BusMove()
    {
        universalMovement.StartMove(moveOBJ, targetPos, () => 
        {
            ShowOtherCat();
        });
    }

    /// <summary>
    /// 显示其他猫猫
    /// </summary>
    public void ShowOtherCat()
    {
        CAT_42.GetComponent<BoxCollider2D>().enabled = true;
        CAT_41.GetComponent<BoxCollider2D>().enabled = true;
        CAT_133.GetComponent<SpriteRenderer>().enabled = true;
        CAT_133.GetComponent<BoxCollider2D>().enabled = true;
    }


    /// <summary>
    /// 车的点击
    /// </summary>
    public void CarClick()
    {
        carClose.SetActive(false);
        carOpen.SetActive(true);
    }

   /// <summary>
   /// 纸箱的点击
   /// </summary>
    public void BoxClick()
    {
        carOpen.SetActive(false);
        catCat.SetActive(true);
        cat_53.enabled = true;
        StartCoroutine(DelayOpenCat());
    }

    IEnumerator DelayOpenCat()
    {
        yield return new WaitForSeconds(1);
        cat_53.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void JuageCat_53()
    {
        carClose.SetActive(false);
        carOpen.SetActive(false);
        catCat.SetActive(true);
        cat_53.enabled = true;
    }
}
