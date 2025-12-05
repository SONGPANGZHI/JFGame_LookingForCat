using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private PuzzleManager puzzleGame;          // 拼图
    [SerializeField]
    private NineSquareGridPuzzle NineSquareGridPuzzle;      // 九宫格 拼图

    [SerializeField]
    private SequenceButtonGame SequenceButtonGame;


    public TMP_Text foundCountText;

    [Header("设置UI参数")]
    public Image setBG;
    public Button SFX_BTN;
    public Button BGM_BTN;
    public Button Set_BTN;

    public GameObject setPlane;

    public Transform moveTrans;
    public Transform targetPosition;

    [Header("提醒界面")]
    public CameraTouchDrag cameraOBJ;

    private Vector3 startPosition;
    private bool setOpen = false;
    private InputManager inputManager;

    [Header("胜利")]
    public SkeletonGraphic winPlane;

    [SerializeField]
    private int catAllNumber = 150;

    [SerializeField] private SpriteRenderer coloring;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        inputManager = GetComponent<InputManager>();
        startPosition = moveTrans.position;

        SFX_BTN.onClick.AddListener(SFXSwitchClick);
        BGM_BTN.onClick.AddListener(BGMSwitchClick);
        Set_BTN.onClick.AddListener(SettingSwitchClick);

        InitStates();
    }

    #region 主界面设置功能逻辑

    /// <summary>
    /// 初始化 按钮状态
    /// </summary>
    public void InitStates()
    {
        if (PlayerPrefs.GetInt(MusicManager.SFXKey) == 0)
            SFX_BTN.transform.GetChild(0).gameObject.SetActive(false);
        else
            SFX_BTN.transform.GetChild(0).gameObject.SetActive(true);

        if (PlayerPrefs.GetInt(MusicManager.BGMKey) == 0)
            BGM_BTN.transform.GetChild(0).gameObject.SetActive(false);
        else
            BGM_BTN.transform.GetChild(0).gameObject.SetActive(true);
    }

    /// <summary>
    /// SFX 开关
    /// </summary>
    private void SFXSwitchClick()
    {
        if (PlayerPrefs.GetInt(MusicManager.SFXKey) == 0)
        {
            //关闭
            PlayerPrefs.SetInt(MusicManager.SFXKey, 1);
            SFX_BTN.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            //打开
            PlayerPrefs.SetInt(MusicManager.SFXKey, 0);
            SFX_BTN.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// BGM 开关
    /// </summary>
    private void BGMSwitchClick()
    {
        if (PlayerPrefs.GetInt(MusicManager.BGMKey) == 0)
        {
            //关闭
            PlayerPrefs.SetInt(MusicManager.BGMKey, 1);
            BGM_BTN.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            //打开
            PlayerPrefs.SetInt(MusicManager.BGMKey, 0);
            BGM_BTN.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Set 开关
    /// </summary>
    private void SettingSwitchClick()
    {
        //设置背景显示 找猫猫标题移动 打开两个按钮
        if (setOpen)
        {
            //关闭 设置
            setOpen = false;
            StartCoroutine(CloseFillProgressBar());
        }
        else
        {
            //打开 设置
            setOpen = true;
            StartCoroutine(FillProgressBar());
        }
        Set_BTN.interactable = false;
    }

    /// <summary>
    /// 标签向右移动
    /// </summary>
    /// <returns></returns>
    IEnumerator FillProgressBar()
    {
        float duration = 1f; // 1秒钟
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            moveTrans.position = Vector3.Lerp(startPosition, targetPosition.position, elapsedTime);
            setBG.fillAmount = Mathf.Clamp01(elapsedTime / duration);
            yield return null;
        }

        OpenBtton(true);
        moveTrans.position = targetPosition.position;
        setBG.fillAmount = 1f; // 确保最终值为1
        Set_BTN.interactable = true;
    }

    /// <summary>
    /// 标签向左移动
    /// </summary>
    /// <returns></returns>
    IEnumerator CloseFillProgressBar()
    {
        OpenBtton(false);
        float duration = 1f; // 1秒钟
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            moveTrans.position = Vector3.Lerp(targetPosition.position, startPosition, elapsedTime / duration);
            setBG.fillAmount = Mathf.Clamp01(1f - (elapsedTime / duration));
            yield return null;
        }

        moveTrans.position = startPosition;
        setBG.fillAmount = 0f;
        Set_BTN.interactable = true;

    }

    /// <summary>
    /// 打开 按钮
    /// </summary>
    public void OpenBtton(bool active)
    {
        SFX_BTN.gameObject.SetActive(active);
        BGM_BTN.gameObject.SetActive(active);
    }

    #endregion

    #region 按ESC键 设置界面

    /// <summary>
    /// 打开设置界面
    /// </summary>

    public void OpenSettingPlane(int index)
    {
        if (index == 1)
        {
            OtherParameters(false);
            setPlane.SetActive(true);
        }
        else
        {
            openIndex = 0;
            OtherParameters(true);
            setPlane.SetActive(false);
            MusicManager.Instance.SetBackgroundMusicForPause(false);
        }

        Time.timeScale = 1f;
    }

    int openIndex = 0;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            openIndex += 1;
            OpenSettingPlane(openIndex);
        }
    }

    /// <summary>
    /// 在场景中调用
    /// </summary>
    public void SetPlaneClose()
    {
        openIndex += 1;
        OpenSettingPlane(openIndex);
    }

    #endregion

    #region 小游戏界面 提醒通关 摄像机参数

   
    /// <summary>
    /// 其他参数 摄像机 可以点击
    /// </summary>
    public void OtherParameters(bool _active)
    {
        cameraOBJ.enabled = _active;
        inputManager.SetUIOpenState(!_active);
    } 


    #endregion


    /// <summary>
    /// 更新猫猫数量 UI
    /// </summary>
    public void UpdateProgressUI()
    {
        int catCount = GameManager.Instance.progressManager.FoundCatCount;
        foundCountText.text = $"{catCount}/{GameManager.Instance.progressManager.TotalCatCount}";

        if (catCount >= catAllNumber)
        {
            //添加Steam 成就
            AchievementManager.Instance.UnlockAchievement("ACH_FINDALLCATS");

            inputManager.SetUIOpenState(true);

            // 胜利
            winPlane.gameObject.SetActive(true);
            winPlane.AnimationState.SetAnimation(0, "animation", true);
            StartCoroutine(FadeIn());
            Invoke("CloseWinObject",5f);
        }
    }

    public void CloseWinObject()
    {
        winPlane.gameObject.SetActive(false);
    }

    public float duration = 5f;
    IEnumerator FadeIn()
    {
        coloring.gameObject.SetActive(true);
        Color c = coloring.color;
        c.a = 0;
        coloring.color = c;

        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / duration);
            coloring.color = c;
            yield return null;
        }

        // 保证最终完全显示
        c.a = 1;
        coloring.color = c;
    }

    // ID_87 开始拼图
    public void StartPuzzle()
    {
        bool isCompleted = GameManager.Instance.progressManager.IsCatFound(87);

        if (isCompleted) return;
        else puzzleGame.OpenPuzzle();
    }
    

    // ID_77_78 网球游戏
    public void StartGrid()
    {
        bool isCompleted = GameManager.Instance.progressManager.IsCatFound(77);
        bool Completed = GameManager.Instance.progressManager.IsCatFound(78);

        if(isCompleted || Completed) return;
        else GridManager.Instance.StartPlay();
    }

    /// <summary>
    /// 九宫格拼图
    /// </summary>
    public void Nine_SquareGridPuzzle()
    {
        bool isCompleted = GameManager.Instance.progressManager.IsCatFound(112);
        if (isCompleted) return;
        else NineSquareGridPuzzle.StartPlayPuzzle();
    }

    public void PlaySharkTeeth()
    {
        bool isCompleted = GameManager.Instance.progressManager.IsCatFound(85);

        if (PlayerPrefs.HasKey("ShaarkTeethKey"))
            return;

        //if (isCompleted && PlayerPrefs.HasKey("ShaarkTeethKey")) return;
        else SequenceButtonGame.OpenSharkTeethPlane();

    }

    /// <summary>
    /// 寻找小游戏 小猫
    /// </summary>
    public void FindSmallGameCat()
    {
        bool isCompleted_112 = GameManager.Instance.progressManager.IsCatFound(112);
        bool isCompleted_122 = GameManager.Instance.progressManager.IsCatFound(122);
        bool isCompleted_87 = GameManager.Instance.progressManager.IsCatFound(87);
        bool isCompleted_85 = GameManager.Instance.progressManager.IsCatFound(85);

        //添加Steam 成就
        if (isCompleted_112 && isCompleted_122 && isCompleted_87 && isCompleted_85)
            AchievementManager.Instance.UnlockAchievement("ACH_SMALLGAMECAT");
    }


    public void ShowCatFoundPopup(CatBase cat)
    {
        Debug.Log($"猫猫 #{cat.catID} 被找到");
    }

    
}
