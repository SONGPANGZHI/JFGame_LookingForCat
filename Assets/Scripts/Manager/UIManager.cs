using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private PuzzleManager puzzleGame;          // 拼图

    [Header("UI元素")]
    public TMP_Text foundCountText;

    public Image setBG;
    public Button SFX_BTN;
    public Button BGM_BTN;
    public Button Set_BTN;

    public Transform moveTrans;
    public Transform targetPosition;

    private Vector3 startPosition;
    private bool setOpen = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        startPosition = moveTrans.position;

        SFX_BTN.onClick.AddListener(SFXSwitchClick);
        BGM_BTN.onClick.AddListener(BGMSwitchClick);
        Set_BTN.onClick.AddListener(SettingSwitchClick);

        InitStates();
    }

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
        if(setOpen)
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


    /// <summary>
    /// 更新猫猫数量 UI
    /// </summary>
    public void UpdateProgressUI()
    {
        foundCountText.text = $"{GameManager.Instance.progressManager.FoundCatCount}/{GameManager.Instance.progressManager.TotalCatCount}";
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

        if(isCompleted|| Completed) return;
        else GridManager.Instance.StartPlay();
    }


    public void ShowCatFoundPopup(CatBase cat)
    {
        //foundCatImage.sprite = cat.foundSprite;
        //foundCatName.text = $"猫猫 #{cat.catID}";
        //foundPopup.SetActive(true);

        //// 3秒后自动关闭
        //StartCoroutine(HidePopupAfterDelay(3f));
        Debug.Log($"猫猫 #{cat.catID} 被找到");
    }

    private IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //foundPopup.SetActive(false);
    }

    public void OnCollectibleButtonClick()
    {
        // 打开收集品图鉴
        // 实现图鉴逻辑
    }
}
