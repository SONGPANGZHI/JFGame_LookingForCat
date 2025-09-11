using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ID_10_12_11_116 猫猫 海盗船逻辑  082
/// </summary>
public class SevenSeasDeluxe : MonoBehaviour
{
    public static SevenSeasDeluxe Instance;

    public List<GameObject> sailList;

    public VisibleCat ID_010_Cat;

    public InteractiveCat ID_011_Cat;

    public VisibleCat ID_012_Cat;

    public Transform targetPosition;
    public GameObject sprayOBJ;
    public GameObject ship;
    public GameObject otherCat;

    private int clickNum;
    private Vector3 startPosition;
    private float moveSpeed = 0.5f; // 移动速度
    private bool isMoving = false;

    [Header("ID_082")]
    public SkeletonAnimation lightAnim;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public void Start()
    {
        startPosition = ship.transform.position;

        bool isCompleted = GameManager.Instance.progressManager.IsCatFound(12);
        bool isCompleted_082 = GameManager.Instance.progressManager.IsCatFound(82);

        if (isCompleted)
        {
            for (int i = 0; i < sailList.Count; i++) { sailList[i].gameObject.SetActive(true); }
            ship.transform.position = targetPosition.position;

            ID_012_Cat.GetComponent<SpriteRenderer>().enabled = true;
            OpenCatShow();
            sprayOBJ.SetActive(true);
        }

        if (isCompleted_082)
            PlayCatAnim_082();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sailClick"></param>
    public void ClickSail_01(Transform sailClick)
    {
        clickNum += 1;
        sailClick.gameObject.SetActive(false);
        sailList[0].SetActive(true);
        IsCreateID_012();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sailClick"></param>
    public void ClickSail_02(Transform sailClick)
    {
        clickNum += 1;
        sailClick.gameObject.SetActive(false);
        sailList[1].SetActive(true);
        IsCreateID_012();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sailClick"></param>
    public void ClickSail_03(Transform sailClick)
    {
        clickNum += 1;
        sailClick.gameObject.SetActive(false);
        sailList[2].SetActive(true);
        IsCreateID_012();
    }

    /// <summary>
    /// 是否生成 ID_012 猫
    /// </summary>
    public void IsCreateID_012()
    {
        if (clickNum >= 3)
        {
            ID_012_Cat.GetComponent<SpriteRenderer>().enabled = true;
            ID_012_Cat.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public void StartMove()
    {
        otherCat.SetActive(true);

        if (!isMoving)
        {
            StartCoroutine(MoveToTarget());
        }

    }


    // 碎片移动到目标位置
    IEnumerator MoveToTarget()
    {
        isMoving = true;

        float progress = 0f;

        while (progress < 1f)
        {
            progress += Time.deltaTime * moveSpeed;

            // 移动位置
            ship.transform.position = Vector3.Lerp(startPosition, targetPosition.position, progress);
            yield return null;
        }

        // 确保最终位置和旋转完全匹配
        ship.transform.position = targetPosition.position;

        OpenCatShow();

         isMoving = false;
    }


    public void OpenCatShow()
    {
        for (int i = 0; i < otherCat.transform.childCount; i++)
        {
            otherCat.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
            otherCat.transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = true;
        }

        ID_010_Cat.GetComponent<SpriteRenderer>().enabled = true;
        ID_010_Cat.GetComponent<BoxCollider2D>().enabled = true;
        ID_011_Cat.GetComponent<SpriteRenderer>().enabled = true;
        ID_011_Cat.GetComponent<BoxCollider2D>().enabled = true;
    }


    /// <summary>
    /// 播放082猫猫 动画
    /// </summary>
    public void PlayCatAnim_082()
    {
        lightAnim.gameObject.SetActive(true);
        lightAnim.state.SetAnimation(0,"",true);
    }
}
