using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 特殊事件猫猫
/// </summary>
public class SomeCatInteractionEvents : MonoBehaviour
{
    public static SomeCatInteractionEvents Instance;
    public UniversalMovementController moveController;

    [Header("ID_063")]
    public List<Transform> targetPos;
    public List<Transform> moveOBJ;
    public Collider2D CatID_063;

    private List<Transform> startPos = new List<Transform>();
    private bool left_move = false;
    private bool right_move = false;

    [Header("ID_62_51 热气球")]

    public List<SpriteRenderer> balloonSprites;
    public List<GameObject> pumpOBJ;
    public Collider2D catID_51;
    public Collider2D catID_62;
    private int balloonIndex = 0;

    [Header("垃圾桶切图")]
    public Sprite bin_Sprite;
    public List<SpriteRenderer> binList;

    [Header("ID_125")]
    public SpriteRenderer catID_125;


    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        Init_ID_063();
    }


    private void Start()
    {
        CheckCatUnlock_063();
        JuageCatIDUnlock_62();
        CheckBinCatSprite();
        CheckCatID_125();
        CheckCatID_146();
    }

    #region ID_063

    public void Init_ID_063()
    {
        startPos.Clear();
        startPos = moveOBJ;

        for (int i = 0; i < moveOBJ.Count; i++)
        {
            startPos[i].position = moveOBJ[i].position;
        }
    }

    /// <summary>
    /// 点击左边船桨
    /// </summary>
    public void ClickOarLeft()
    {
        moveController.StartMoveWithSpeed(moveOBJ[0], targetPos[0],2f, () =>
        {
            left_move = true;
            JudageCatClick();
        });
    }

    /// <summary>
    /// 点击右边船桨
    /// </summary>
    public void ClickOarRight()
    {
        moveController.StartMoveWithSpeed(moveOBJ[1], targetPos[1], 2f, () =>
        {
            right_move = true;
            JudageCatClick();
        });
    }

    /// <summary>
    /// 判断猫猫是否可以点击
    /// </summary>
    public void JudageCatClick()
    {
        if (right_move && left_move)
        {
            CatID_063.enabled = true;
        }
    }

    /// <summary>
    /// 检查猫猫解锁
    /// </summary>
    public void CheckCatUnlock_063()
    {
        bool isCompleted_063 = GameManager.Instance.progressManager.IsCatFound(63);
        if (isCompleted_063)
        {
            moveOBJ[0].position = targetPos[0].position;
            moveOBJ[1].position = targetPos[1].position;
        }
    }

    #endregion


    #region 热气球


    /// <summary>
    /// 点击打气泵
    /// </summary>
    public void ClickPump()
    {
        if (balloonIndex >= 3)
            return;

        balloonIndex += 1;
        SwitchBalloon(balloonIndex);
    }


    public void SwitchBalloon(int index)
    {
        for (int i = 0; i < balloonSprites.Count; i++)
        {
            if (i == index)
            {
                balloonSprites[i].enabled = true;
                pumpOBJ[i].SetActive(true);
            }
            else
            {
                balloonSprites[i].enabled = false;
                pumpOBJ[i].SetActive(false);
            }
        }

        if (index == 3)
            OpenCatID_62();
    }

    /// <summary>
    /// 打开 62 号猫碰撞体
    /// </summary>
    public void OpenCatID_62()
    {
        catID_62.enabled = true;
    }

    /// <summary>
    /// 打开51号猫碰撞体
    /// </summary>
    public void OpenCatID_51()
    {
        catID_51.enabled = true;
        catID_51.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void JuageCatIDUnlock_62()
    {
        bool isCompleted_62 = GameManager.Instance.progressManager.IsCatFound(62);
        if (isCompleted_62)
        {
            SwitchBalloon(3);
            OpenCatID_51();
        }
    }

    #endregion

    #region 垃圾桶切换图片

    /// <summary>
    /// 切换垃圾桶图片
    /// </summary>
    /// <param name="ID"></param>
    public void SwitchBinSprite(int ID)
    {
        binList[ID].GetComponent<SpriteRenderer>().sprite = bin_Sprite;
        PlayerPrefs.SetInt("BinCat_" + ID.ToString(), ID);
    }

    /// <summary>
    /// 检查垃圾桶
    /// </summary>
    public void CheckBinCatSprite()
    {
        for (int i = 0; i < 3; i++)
        {
            int savedID = PlayerPrefs.GetInt("BinCat_" + i.ToString());

            if (savedID == i)
            {
                binList[i].GetComponent<SpriteRenderer>().sprite = bin_Sprite;
            }
        }
    }

    #endregion

    #region ID_125

    /// <summary>
    /// 点击木板子
    /// </summary>
    public void ClickPlank()
    {
        if (PlayerPrefs.HasKey("CrabKey_12"))
        {
            Invoke("OpenCatID_125",0.2f); 
        }
        else
            return;
    }

    public void OpenCatID_125()
    {
        catID_125.enabled = true;
        catID_125.GetComponent<Collider2D>().enabled = true;
    }

    public void CheckCatID_125()
    {
        bool isCompleted_125 = GameManager.Instance.progressManager.IsCatFound(125);
        if (isCompleted_125)
        {
            OpenCatID_125();
        }
    }

    #endregion

    #region 水神

    public SkeletonAnimation goods_146;
    public SkeletonAnimation catID_146;

    /// <summary>
    /// 点击交互物品 146 号猫
    /// </summary>
    public void ClickGoods_146()
    {

        // 设置动画并获取 TrackEntry
        TrackEntry trackEntry = goods_146.state.SetAnimation(0, "Stay", false);
        // 添加完成事件监听
        trackEntry.Complete += OnAnimationComplete;


    }

    void OnAnimationComplete(TrackEntry trackEntry)
    {
        // 移除事件监听，避免重复调用
        trackEntry.Complete -= OnAnimationComplete;
        goods_146.GetComponent<BoxCollider2D>().enabled = false;
        goods_146.gameObject.SetActive(false);

        catID_146.GetComponent<MeshRenderer>().enabled = true;
        catID_146.GetComponent<SkeletonAnimation>().enabled = true;
        catID_146.GetComponent<Collider2D>().enabled = true;
        //StartCoroutine(GraduallyDisplayOtherCat());
    }

    public void CheckCatID_146()
    {
        bool isCompleted_146 = GameManager.Instance.progressManager.IsCatFound(146);
        if (isCompleted_146)
        {
            goods_146.gameObject.SetActive(false);
            catID_146.GetComponent<MeshRenderer>().enabled = true;
            catID_146.GetComponent<SkeletonAnimation>().enabled = true;
            catID_146.GetComponent<Collider2D>().enabled = true;
        }
    }

    #endregion

}
