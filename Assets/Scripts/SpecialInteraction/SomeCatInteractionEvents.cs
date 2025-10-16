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
}
