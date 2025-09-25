using Spine;
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
    public SkeletonAnimation cat_038;

    private int clickNum;
    private Vector3 startPosition;
    private float moveSpeed = 0.5f; // 移动速度
    private bool isMoving = false;

    [Header("ID_082")]
    public SkeletonAnimation lightAnim;

    [Header("ID_113")]
    public SkeletonAnimation catlightAnim;

    [Header("ID_073")]
    public SkeletonAnimation cat_072;
    public GameObject catTail;

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
        bool isCompleted_030 = GameManager.Instance.progressManager.IsCatFound(30);

        if (isCompleted)
        {
            for (int i = 0; i < sailList.Count; i++) { sailList[i].gameObject.SetActive(true); }
            ship.transform.position = targetPosition.position;

            ID_012_Cat.GetComponent<SpriteRenderer>().enabled = true;
            OpenCatShow();
            sprayOBJ.SetActive(true);
            ShowCat_038(true);
        }

        if (isCompleted_082)
            PlayCatAnim_082();

        if (isCompleted_030)
            PlayCatAnim_031030();

        if (GameManager.Instance.progressManager.IsCatFound(31))
            ChangeCatColor();

        if (GameManager.Instance.progressManager.IsCatFound(30))
            ChangeCatColor_30();

        if (GameManager.Instance.progressManager.IsCatFound(113))
            PlayCatAnim_113();

        if (GameManager.Instance.progressManager.IsCatFound(72))
            ClickCat_072();
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
        ShowCat_038(true);

        if (!isMoving)
        {
            //UniversalMovementController.Instance.CameraMove(cameraTargetPos);
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
    /// 点击72号猫猫交互
    /// </summary>
    public void ClickCat_072()
    {
        catTail.SetActive(false);
        cat_072.GetComponent<MeshRenderer>().enabled = true;
        cat_072.GetComponent<SkeletonAnimation>().enabled = true;
        cat_072.GetComponent<Collider2D>().enabled = true;
    }


    /// <summary>
    /// 显示38号猫猫
    /// </summary>
    public void ShowCat_038(bool _isAtive)
    {
        cat_038.GetComponent<MeshRenderer>().enabled = _isAtive;
        cat_038.GetComponent<SkeletonAnimation>().enabled = _isAtive;
        cat_038.GetComponent<Collider2D>().enabled = _isAtive;
    }


    /// <summary>
    /// 播放082猫猫 动画
    /// </summary>
    public void PlayCatAnim_082()
    {
        lightAnim.gameObject.SetActive(true);
        lightAnim.state.SetAnimation(0, "Sports", true);
    }

    /// <summary>
    /// 播放113猫猫 动画
    /// </summary>
    /// 
    public void PlayCatAnim_113()
    {
        catlightAnim.gameObject.SetActive(true);
        catlightAnim.state.SetAnimation(0,"Sports",true);
    }

    [Header("30-31猫猫")]

    public SkeletonAnimation cat_30;
    public SkeletonAnimation cat_31;        //钓鱼猫
    private bool _startFinsh = false;

    /// <summary>
    /// 点击钓鱼动画
    /// </summary>
    public void ClickPlayFishAnim()
    {
        _startFinsh = true;
        bool isCompleted = GameManager.Instance.progressManager.IsCatFound(30);
        if (isCompleted && PlayerPrefs.HasKey("InteractiveFishKey")) return;
        else
        {
            // 设置动画并获取 TrackEntry
            TrackEntry trackEntry = cat_31.state.SetAnimation(0, "Sports", false);

            // 添加完成事件监听
            trackEntry.Complete += OnAnimationComplete;
        }
    }

    void OnAnimationComplete(TrackEntry trackEntry)
    {
        
        // 移除事件监听，避免重复调用
        trackEntry.Complete -= OnAnimationComplete;
        PlayCatAnim_031030();
    }

    /// <summary>
    /// 播放03130 动画
    /// </summary>
    public void PlayCatAnim_031030()
    {
        //切换钓鱼动画
        cat_31.state.SetAnimation(0, "Stay", true);

        //美人鱼猫猫显示
        cat_30.GetComponent<MeshRenderer>().enabled = true;
        cat_30.GetComponent<BoxCollider2D>().enabled = true;
        cat_30.enabled = true;
        cat_30.state.SetAnimation(0, "Stay2", true);
        PlayerPrefs.SetString("InteractiveFishKey", "InteractiveFish");

    }

    public void ChangeCatColor()
    {
        cat_31.Skeleton.SetColor(Color.gray);
    }

    public void ChangeCatColor_30()
    {
        cat_30.Skeleton.SetColor(Color.gray);
    }
}
