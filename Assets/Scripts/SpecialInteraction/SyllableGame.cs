using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SyllableGame;

/// <summary>
/// 音节小游戏
/// </summary>
public class SyllableGame : MonoBehaviour
{
    [System.Serializable]
    public class SpineButton2D
    {
        public GameObject noteSprite;         // 按钮对应的音符图片
        public SkeletonAnimation spineAnimation; // Spine动画组件
        public Collider2D clickCollider;         // 2D碰撞器
        public string clickAnimation = "stay";
        public Transform targetPosition;      // 音符移动的目标位置（可选）
    }

    [Header("2D Spine按钮设置")]
    public List<SpineButton2D> spineButtons = new List<SpineButton2D>();

    [Header("正确顺序")]
    public List<int> correctSequence = new List<int> { 5, 6, 7, 2, 4, 1, 3 };

    [Header("动画设置")]
    public float resetDelay = 1f;
    public bool enableHints = true;
    public float noteScaleDuration = 0.5f;    // 音符缩放动画时长
    public float noteMoveDuration = 0.8f;     // 音符移动动画时长
    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("颜色设置")]
    public List<Color> spineColors;

    [Header("猫猫")]
    public Collider2D catID_136;

    private List<int> currentSequence = new List<int>();
    private Dictionary<Collider2D, int> colliderToIndexMap = new Dictionary<Collider2D, int>();
    private Dictionary<int, SpineButton2D> indexToSpineButtonMap = new Dictionary<int, SpineButton2D>();
    private Camera mainCamera;
    private Dictionary<SpineButton2D, Coroutine> activeCoroutines = new Dictionary<SpineButton2D, Coroutine>();

    void Start()
    {
        mainCamera = Camera.main;
        InitializeSpineButtons2D();

        bool FindCat_137 = GameManager.Instance.progressManager.IsCatFound(137);

        if (FindCat_137)
            ShowCat();
    }

    void InitializeSpineButtons2D()
    {
        // 初始化2D Spine按钮
        for (int i = 0; i < spineButtons.Count; i++)
        {
            int buttonIndex = i + 1;
            var spineButton = spineButtons[i];

            colliderToIndexMap[spineButton.clickCollider] = buttonIndex;
            indexToSpineButtonMap[buttonIndex] = spineButton;

            // 初始化音符状态
            if (spineButton.noteSprite != null)
            {
                spineButton.noteSprite.SetActive(false);
                spineButton.noteSprite.transform.localScale = Vector3.zero;
            }
        }

        // 更新提示
        if (enableHints)
        {
            UpdateButtonHints();
        }
    }

    void Update()
    {
        HandleSpineButtonClicks();
    }

    void HandleSpineButtonClicks()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && colliderToIndexMap.ContainsKey(hit.collider))
            {
                int buttonIndex = colliderToIndexMap[hit.collider];
                SpineButton2D spineButton = indexToSpineButtonMap[buttonIndex];

                OnSpineButtonClicked2D(spineButton, buttonIndex);
            }
        }
    }

    void OnSpineButtonClicked2D(SpineButton2D spineButton, int buttonIndex)
    {
        currentSequence.Add(buttonIndex);

        // 播放点击动画
        PlaySpineAnimation(spineButton, spineButton.clickAnimation, false);

        // 启动音符动画
        if (spineButton.noteSprite != null)
        {
            // 如果已经有动画在运行，先停止
            if (activeCoroutines.ContainsKey(spineButton) && activeCoroutines[spineButton] != null)
            {
                StopCoroutine(activeCoroutines[spineButton]);
            }

            // 启动新的动画协程
            var coroutine = StartCoroutine(PlayNoteAnimation(spineButton));
            activeCoroutines[spineButton] = coroutine;
        }

        // 检查序列是否正确
        bool isCorrect = CheckCurrentSequence();

        if (isCorrect)
        {
            // 正确点击
            StartCoroutine(HandleCorrectClick2D(spineButton, buttonIndex));
        }
        else
        {
            // 错误点击
            StartCoroutine(HandleWrongClick2D());
        }
    }

    IEnumerator PlayNoteAnimation(SpineButton2D spineButton)
    {
        if (spineButton.noteSprite == null) yield break;

        // 激活音符
        spineButton.noteSprite.SetActive(true);

        Vector3 startPosition = spineButton.noteSprite.transform.position;
        Vector3 targetPosition = spineButton.targetPosition != null ?
            spineButton.targetPosition.position : startPosition;

        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = Vector3.one;

        float timer = 0f;

        // 缩放和移动动画
        while (timer < Mathf.Max(noteScaleDuration, noteMoveDuration))
        {
            timer += Time.deltaTime;

            // 缩放动画
            if (timer <= noteScaleDuration)
            {
                float scaleProgress = timer / noteScaleDuration;
                float scaleValue = scaleCurve.Evaluate(scaleProgress);
                spineButton.noteSprite.transform.localScale = Vector3.Lerp(startScale, targetScale, scaleValue);
            }

            // 移动动画
            if (timer <= noteMoveDuration)
            {
                float moveProgress = timer / noteMoveDuration;
                float moveValue = moveCurve.Evaluate(moveProgress);
                spineButton.noteSprite.transform.position = Vector3.Lerp(startPosition, targetPosition, moveValue);
            }

            yield return null;
        }

        // 确保最终状态
        spineButton.noteSprite.transform.localScale = targetScale;
        spineButton.noteSprite.transform.position = targetPosition;
    }

    IEnumerator HandleCorrectClick2D(SpineButton2D spineButton, int buttonIndex)
    {
        // 等待点击动画和音符动画播放完成
        yield return new WaitForSeconds(Mathf.Max(0.5f, noteScaleDuration, noteMoveDuration));

        ChangeSpineColor(spineButton, spineColors[0]);
        spineButton.clickCollider.enabled = false;

        // 更新提示
        if (enableHints)
        {
            UpdateButtonHints();
        }

        // 检查是否完成整个序列
        if (currentSequence.Count == correctSequence.Count)
        {
            SequenceCompleted();
        }
    }

    IEnumerator HandleWrongClick2D()
    {
        // 先停止所有正在运行的音符动画
        foreach (var coroutine in activeCoroutines.Values)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
        activeCoroutines.Clear();

        // 设置错误颜色
        foreach (var spineButton in spineButtons)
        {
            ChangeSpineColor(spineButton, spineColors[1]);

            // 立即隐藏音符（不需要动画）
            if (spineButton.noteSprite != null)
            {
                spineButton.noteSprite.SetActive(false);
                spineButton.noteSprite.transform.localScale = Vector3.zero;
            }
        }

        yield return new WaitForSeconds(resetDelay);

        ResetAllSpineButtons();
    }

    bool CheckCurrentSequence()
    {
        // 检查当前序列是否与正确序列的前N个匹配
        for (int i = 0; i < currentSequence.Count; i++)
        {
            if (currentSequence[i] != correctSequence[i])
            {
                return false;
            }
        }
        return true;
    }

    void ResetAllSpineButtons()
    {
        currentSequence.Clear();

        // 停止所有动画
        foreach (var coroutine in activeCoroutines.Values)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
        activeCoroutines.Clear();

        foreach (var spineButton in spineButtons)
        {
            spineButton.clickCollider.enabled = true;
            ChangeSpineColor(spineButton, spineColors[2]);

            // 重置音符状态
            if (spineButton.noteSprite != null)
            {
                spineButton.noteSprite.SetActive(false);
                spineButton.noteSprite.transform.localScale = Vector3.zero;

                // 重置位置到原始位置（如果需要）
                // 注意：这里需要保存原始位置或者在SpineButton2D中添加startPosition字段
            }
        }

        // 更新提示
        if (enableHints)
        {
            UpdateButtonHints();
        }

        Debug.Log("顺序错误，已重置所有Spine动画！");
    }

    void UpdateButtonHints()
    {
        if (!enableHints) return;

        // 重置所有按钮到待机状态（除了已完成的）
        for (int i = 0; i < spineButtons.Count; i++)
        {
            int buttonIndex = i + 1;
            var spineButton = spineButtons[i];

            // 如果按钮已经完成，跳过
            if (!spineButton.clickCollider.enabled) continue;
        }
    }

    void SequenceCompleted()
    {
        Debug.Log("恭喜！顺序正确！所有Spine动画完成！");

        // 这里可以添加完成后的特效，比如播放庆祝动画等
        StartCoroutine(PlayCompletionEffects());
    }

    IEnumerator PlayCompletionEffects()
    {
        // 播放庆祝效果
        foreach (var spineButton in spineButtons)
        {
            // 可以添加额外的庆祝动画效果
            if (spineButton.noteSprite != null)
            {
                // 音符跳动效果
                StartCoroutine(PlayCelebrationEffect(spineButton.noteSprite.transform));
            }
        }

        yield return new WaitForSeconds(1f);

        // 可以在这里触发其他完成事件
        Debug.Log("任务完成！");

        ShowCat();

    }

    /// <summary>
    /// 显示猫猫
    /// </summary>
    public void ShowCat()
    {
        for (int i = 0; i < spineButtons.Count; i++)
        {
            spineButtons[i].clickCollider.enabled = false;
            spineButtons[i].noteSprite.SetActive(false);
            ChangeSpineColor(spineButtons[i], spineColors[2]);
            if (i == 6)
            {
                spineButtons[i].clickCollider.gameObject.SetActive(false);
            }

        }

        catID_136.GetComponent<MeshRenderer>().enabled = true;
        catID_136.GetComponent<SkeletonAnimation>().enabled = true;
        catID_136.enabled = true;
    }


    IEnumerator PlayCelebrationEffect(Transform noteTransform)
    {
        Vector3 originalScale = noteTransform.localScale;
        float timer = 0f;
        float duration = 0.3f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;
            float scale = originalScale.x * (1f + Mathf.Sin(progress * Mathf.PI * 4) * 0.2f);
            noteTransform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        noteTransform.localScale = originalScale;
    }

    void PlaySpineAnimation(SpineButton2D spineButton, string animationName, bool loop)
    {
        if (spineButton.spineAnimation != null && !string.IsNullOrEmpty(animationName))
        {
            spineButton.spineAnimation.AnimationState.SetAnimation(0, animationName, loop);
        }
    }

    public void ChangeSpineColor(SpineButton2D spineButton, Color color)
    {
        if (spineButton.spineAnimation != null)
        {
            spineButton.spineAnimation.Skeleton.SetColor(color);
        }
    }

    
}
