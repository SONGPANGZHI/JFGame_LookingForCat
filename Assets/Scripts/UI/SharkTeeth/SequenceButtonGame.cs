using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI层 鲨鱼牙齿小游戏
/// </summary>
public class SequenceButtonGame : MonoBehaviour
{
    [Header("游戏设置")]
    public int maxButtons = 5; // 每行按钮数量
    public List<int> topSequence = new List<int> { 3, 5, 4, 2 }; // 上方正确顺序
    public List<int> bottomSequence = new List<int> { 4, 3, 2, 1 }; // 下方正确顺序

    [Header("动画设置")]
    public float moveDistance = 100f; // 向下移动距离
    public float moveDuration = 0.2f; // 移动动画时长
    public float returnDuration = 0.15f; // 返回动画时长

    [Header("UI引用")]
    public Transform topButtonPanel;
    public Transform bottomButtonPanel;
    public TMP_Text feedbackText;

    [Header("猫猫配置")]
    public GameObject closeBG_BTN;
    public SpriteRenderer catRender;


    private List<Button> topButtons = new List<Button>();
    private List<Button> bottomButtons = new List<Button>();
    private List<RectTransform> topButtonRects = new List<RectTransform>();
    private List<RectTransform> bottomButtonRects = new List<RectTransform>();
    private List<Vector2> topButtonOriginalPositions = new List<Vector2>();
    private List<Vector2> bottomButtonOriginalPositions = new List<Vector2>();

    private List<int> currentTopSequence = new List<int>();
    private List<int> currentBottomSequence = new List<int>();
    private bool gameActive = true;
    private bool isFailed = false;

    void Start()
    {
        bool isCompleted = GameManager.Instance.progressManager.IsCatFound(85);

        if (isCompleted)
            CatSetParam();

        InitializeGame();
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    public void OpenSharkTeethPlane()
    {
        closeBG_BTN.SetActive(true);
        UIManager.Instance.OtherParameters(false);
    }

    /// <summary>
    /// 猫猫设置参数
    /// </summary>
    public void CatSetParam()
    {
        catRender.enabled = true;
        catRender.GetComponent<Collider2D>().enabled = true;
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    public void ClosePlane()
    {
        ResetGame();
        UIManager.Instance.OtherParameters(true);
        closeBG_BTN.SetActive(false);
        CatSetParam();
    }


    void InitializeGame()
    {
        // 获取按钮引用和初始位置
        GetButtonsAndPositions();

        // 设置按钮点击事件
        SetupButtonListeners();

        // 重置游戏状态
        ResetGame();
    }

    void GetButtonsAndPositions()
    {
        topButtons.Clear();
        bottomButtons.Clear();
        topButtonRects.Clear();
        bottomButtonRects.Clear();
        topButtonOriginalPositions.Clear();
        bottomButtonOriginalPositions.Clear();

        // 获取上方按钮
        for (int i = 0; i < maxButtons; i++)
        {
            if (i < topButtonPanel.childCount)
            {
                Button button = topButtonPanel.GetChild(i).GetComponent<Button>();
                RectTransform rectTransform = topButtonPanel.GetChild(i).GetComponent<RectTransform>();

                if (button != null && rectTransform != null)
                {
                    topButtons.Add(button);
                    topButtonRects.Add(rectTransform);
                    topButtonOriginalPositions.Add(rectTransform.anchoredPosition);
                }
            }
        }

        // 获取下方按钮
        for (int i = 0; i < maxButtons; i++)
        {
            if (i < bottomButtonPanel.childCount)
            {
                Button button = bottomButtonPanel.GetChild(i).GetComponent<Button>();
                RectTransform rectTransform = bottomButtonPanel.GetChild(i).GetComponent<RectTransform>();

                if (button != null && rectTransform != null)
                {
                    bottomButtons.Add(button);
                    bottomButtonRects.Add(rectTransform);
                    bottomButtonOriginalPositions.Add(rectTransform.anchoredPosition);
                }
            }
        }
    }

    void SetupButtonListeners()
    {
        // 移除所有现有的监听器
        RemoveAllButtonListeners();

        // 设置上方按钮点击事件
        for (int i = 0; i < topButtons.Count; i++)
        {
            int index = i;
            topButtons[i].onClick.AddListener(() => OnTopButtonClicked(index + 1));
        }

        // 设置下方按钮点击事件
        for (int i = 0; i < bottomButtons.Count; i++)
        {
            int index = i;
            bottomButtons[i].onClick.AddListener(() => OnBottomButtonClicked(index + 1));
        }
    }

    void RemoveAllButtonListeners()
    {
        // 移除上方按钮所有监听器
        foreach (var button in topButtons)
        {
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
            }
        }

        // 移除下方按钮所有监听器
        foreach (var button in bottomButtons)
        {
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
            }
        }
    }

    void OnTopButtonClicked(int buttonNumber)
    {
        if (!gameActive || isFailed) return;

        // 播放按钮按下动画
        StartCoroutine(AnimateButtonPress(topButtonRects[buttonNumber - 1], buttonNumber - 1, true, true));

        currentTopSequence.Add(buttonNumber);
        CheckSequence();
    }

    void OnBottomButtonClicked(int buttonNumber)
    {
        if (!gameActive || isFailed) return;

        // 播放按钮按下动画
        StartCoroutine(AnimateButtonPress(bottomButtonRects[buttonNumber - 1], buttonNumber - 1, false,false));

        currentBottomSequence.Add(buttonNumber);
        CheckSequence();
    }

    Vector2 targetPosition;
    IEnumerator AnimateButtonPress(RectTransform buttonRect, int index, bool isTopButton,bool _JudgeTB)
    {
        // 禁用按钮交互防止重复点击
        if (isTopButton)
            topButtons[index].interactable = false;
        else
            bottomButtons[index].interactable = false;

        Vector2 originalPosition = isTopButton ? topButtonOriginalPositions[index] : bottomButtonOriginalPositions[index];

        if (_JudgeTB)
        {
            //上面牙齿
            targetPosition = originalPosition + new Vector2(0, -moveDistance);
        }
        else
        {
            //下面牙齿
            targetPosition = originalPosition + new Vector2(0, moveDistance);
        }

        // 向下移动动画
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / moveDuration);
            buttonRect.anchoredPosition = Vector2.Lerp(originalPosition, targetPosition, t);
            yield return null;
        }

        buttonRect.anchoredPosition = targetPosition;

        // 等待一段时间后返回（或者根据游戏状态决定是否返回）
        yield return new WaitForSeconds(0.1f);


        // 重新启用按钮交互（如果游戏还在进行中）
        if (gameActive && !isFailed)
        {
            if (isTopButton)
                topButtons[index].interactable = true;
            else
                bottomButtons[index].interactable = true;
        }
    }

    void CheckSequence()
    {
        // 检查是否按错
        if (IsSequenceWrong())
        {
            Fail();
            return;
        }

        // 检查是否完成
        if (IsSequenceComplete())
        {
            Success();
            return;
        }

    }

    bool IsSequenceWrong()
    {
        // 检查上方序列
        for (int i = 0; i < currentTopSequence.Count; i++)
        {
            if (i >= topSequence.Count || currentTopSequence[i] != topSequence[i])
            {
                return true;
            }
        }

        // 检查下方序列
        for (int i = 0; i < currentBottomSequence.Count; i++)
        {
            if (i >= bottomSequence.Count || currentBottomSequence[i] != bottomSequence[i])
            {
                return true;
            }
        }

        return false;
    }

    bool IsSequenceComplete()
    {
        return currentTopSequence.Count == topSequence.Count &&
               currentBottomSequence.Count == bottomSequence.Count;
    }

    void Success()
    {
        gameActive = false;
        isFailed = false;
        feedbackText.text = "恭喜！顺序正确！";
        feedbackText.color = Color.green;

        // 禁用所有按钮点击
        SetButtonsInteractable(false);

        Invoke("ClosePlane", 2f); 
    }

    void Fail()
    {
        gameActive = false;
        isFailed = true;
        feedbackText.text = "按错了！请重新开始";
        feedbackText.color = Color.red;

        // 立即禁用所有按钮点击
        SetButtonsInteractable(false);

        // 显示错误反馈
        StartCoroutine(ShowErrorFeedback());

    }

    IEnumerator ShowErrorFeedback()
    {
        // 让所有按钮闪烁红色表示错误
        foreach (var button in topButtons)
        {
            if (button != null)
                button.image.color = Color.red;
        }
        foreach (var button in bottomButtons)
        {
            if (button != null)
                button.image.color = Color.red;
        }

        yield return new WaitForSeconds(0.5f);

        // 恢复颜色
        foreach (var button in topButtons)
        {
            if (button != null)
                button.image.color = Color.white;
        }
        foreach (var button in bottomButtons)
        {
            if (button != null)
                button.image.color = Color.white;
        }

        ResetGame();

    } 

    void SetButtonsInteractable(bool interactable)
    {
        // 设置上方按钮可交互状态
        for (int i = 0; i < topButtons.Count; i++)
        {
            if (topButtons[i] != null)
            {
                topButtons[i].interactable = interactable;
                // 如果禁用交互，将按钮移回原位
                if (!interactable && topButtonRects[i] != null)
                {
                    topButtonRects[i].anchoredPosition = topButtonOriginalPositions[i];
                }
            }
        }

        // 设置下方按钮可交互状态
        for (int i = 0; i < bottomButtons.Count; i++)
        {
            if (bottomButtons[i] != null)
            {
                bottomButtons[i].interactable = interactable;
                // 如果禁用交互，将按钮移回原位
                if (!interactable && bottomButtonRects[i] != null)
                {
                    bottomButtonRects[i].anchoredPosition = bottomButtonOriginalPositions[i];
                }
            }
        }
    }

    public void ResetGame()
    {
        currentTopSequence.Clear();
        currentBottomSequence.Clear();
        gameActive = true;
        isFailed = false;
        feedbackText.text = "请按照正确顺序点击按钮";
        feedbackText.color = Color.white;

        // 重新设置按钮监听器
        SetupButtonListeners();

        // 启用所有按钮并重置状态
        SetButtonsInteractable(true);

        // 重置按钮颜色和位置
        ResetAllButtonPositions();

        // 重置按钮颜色
        foreach (var button in topButtons)
        {
            if (button != null)
            {
                button.image.color = Color.white;
            }
        }
        foreach (var button in bottomButtons)
        {
            if (button != null)
            {
                button.image.color = Color.white;
            }
        }
    }

    void ResetAllButtonPositions()
    {
        // 重置上方按钮位置
        for (int i = 0; i < topButtonRects.Count; i++)
        {
            if (topButtonRects[i] != null && i < topButtonOriginalPositions.Count)
            {
                topButtonRects[i].anchoredPosition = topButtonOriginalPositions[i];
            }
        }

        // 重置下方按钮位置
        for (int i = 0; i < bottomButtonRects.Count; i++)
        {
            if (bottomButtonRects[i] != null && i < bottomButtonOriginalPositions.Count)
            {
                bottomButtonRects[i].anchoredPosition = bottomButtonOriginalPositions[i];
            }
        }
    }

    // 添加空检查，避免运行时错误
    void OnValidate()
    {
        if (topSequence == null) topSequence = new List<int>();
        if (bottomSequence == null) bottomSequence = new List<int>();

        // 确保移动距离为负值（向下移动）
        if (moveDistance > 0) moveDistance = -moveDistance;
    }
}
