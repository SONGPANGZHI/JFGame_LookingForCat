using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// 游乐园 附近的猫猫 ID_27/28/24/23/22/39
/// </summary>
public class AmusementPark : MonoBehaviour
{
    public static AmusementPark Instance;
    public UniversalMovementController universalMovement;

    public SkeletonAnimation valvaOBJ;             //阀门

    public SkeletonAnimation childrenSlide;        //滑梯

    public List<GameObject> otherCat;       //其他猫猫 在滑梯上 被水流冲出来的小猫

    [SerializeField] private float fadeDuration = 0.5f; // 渐显持续时间
    [SerializeField] private float delayBetweenCats = 2f; // 猫之间的显示延迟


    public List<Transform> targetPos;
    public List<Transform> startPos;


    public void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        // 显示滑梯小猫
        if (PlayerPrefs.HasKey("ChildrenSlideKey"))
        {
            childrenSlide.state.SetAnimation(0, "Huati", true);
            for (int i = 0; i < otherCat.Count; i++)
            {
                Color color = otherCat[i].GetComponent<SpriteRenderer>().color;
                color.a = 1;
                otherCat[i].GetComponent<SpriteRenderer>().color = color;
            }
        }
           

        bool FindCat_027 = GameManager.Instance.progressManager.IsCatFound(27);
        bool FindCat_028 = GameManager.Instance.progressManager.IsCatFound(28);

        if (FindCat_027)
            startPos[0].position = targetPos[0].position;

        if (FindCat_028)
            startPos[1].position = targetPos[1].position;
    }

    /// <summary>
    /// 阀门点击 
    /// </summary>
    public void ValvaClick()
    {
        // 设置动画并获取 TrackEntry
        TrackEntry trackEntry = valvaOBJ.state.SetAnimation(0, "Sports", false);

        // 添加完成事件监听
        trackEntry.Complete += OnAnimationComplete;


    }

    void OnAnimationComplete(TrackEntry trackEntry)
    {
        // 移除事件监听，避免重复调用
        trackEntry.Complete -= OnAnimationComplete;

        childrenSlide.state.SetAnimation(0, "Huati", true);
        StartCoroutine(GraduallyDisplayOtherCat());
    }


    // 猫猫移动
    public void CatMove_27()
    {
        universalMovement.StartMove(startPos[0], targetPos[0], () => 
        {
            JudgeValvaClick();
        });
    }

    public bool isMoving = false;
    public void CatMove_28()
    {   
        if(!isMoving)
            StartCoroutine(Move());
    }
    IEnumerator Move()
    {
        isMoving = true;
        float progress = 0f;
        float moveSpeed = 1f;
        Vector3 startPosition = startPos[1].position;

        // 移动过程
        while (progress < 1f)
        {
            progress += Time.deltaTime * moveSpeed;
            startPos[1].position = Vector3.Lerp(startPosition, targetPos[1].position, progress);
            yield return null;
        }

        // 确保最终位置准确
        startPos[1].position = targetPos[1].position;
        JudgeValvaClick();
        isMoving = true;

    }

    /// <summary>
    /// 判断水阀是否可以点击
    /// </summary>
    public void JudgeValvaClick()
    {
        bool FindCat_027 = GameManager.Instance.progressManager.IsCatFound(27);
        bool FindCat_028 = GameManager.Instance.progressManager.IsCatFound(28);

        if(FindCat_027 && FindCat_028)
        {
            valvaOBJ.GetComponent<Collider2D>().enabled = true;
        }
    }

    public void PlayChildrenSlideAnim()
    {
        //bool FindCat_022 = GameManager.Instance.progressManager.IsCatFound(22);
        //bool FindCat_023 = GameManager.Instance.progressManager.IsCatFound(23);
        //bool FindCat_024 = GameManager.Instance.progressManager.IsCatFound(24);

        //if(FindCat_022)
        //    otherCat[0].GetComponent<SpriteRenderer>().enabled = false;

        //if (FindCat_023)
        //    otherCat[1].GetComponent<SpriteRenderer>().enabled = false;

        //if (FindCat_024)
        //    otherCat[2].GetComponent<SpriteRenderer>().enabled = false;


        //if (FindCat_022 && FindCat_023 && FindCat_024)
        //{
        //    childrenSlide.state.SetAnimation(0, "Sports", true);
        //}

    }

    /// <summary>
    /// 逐渐显示其他猫猫
    /// </summary>
    IEnumerator GraduallyDisplayOtherCat()
    {
        foreach (GameObject cat in otherCat)
        {
            // 开始渐显当前猫猫
            yield return StartCoroutine(FadeInCat(cat));

            // 等待指定时间  显示猫
            yield return new WaitForSeconds(delayBetweenCats);
        }


    }

    IEnumerator FadeInCat(GameObject catSprite)
    {
        float elapsedTime = 0f;
        Color color = catSprite.GetComponent<SpriteRenderer>().color;

        while (elapsedTime < fadeDuration)
        {
            // 计算当前的透明度（从0到1）
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            catSprite.GetComponent<SpriteRenderer>().color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终完全显示
        color.a = 1f;
        catSprite.GetComponent<SpriteRenderer>().color = color;

        catSprite.GetComponent<BoxCollider2D>().enabled = true;

        PlayerPrefs.SetString("ChildrenSlideKey", "ChildrenSlide");
    }

}
