using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FootManager : MonoBehaviour
{
    [Header("图片序列")]
    public SpriteRenderer[] footSprite; 
    [Header("过渡时间")]
    public float fadeDuration = 1.5f; 

    private int currentIndex = 0; // 当前显示图片的索引
    private bool isTransitioning = false; // 防止在过渡期间重复点击

    void Start()
    {
        bool complete = GameManager.Instance.progressManager.IsCatFound(39);
        if (complete)
        {
            // 初始化：只显示第一张图片，其他隐藏
            for (int i = 0; i < footSprite.Length; i++)
            {
                SetImageAlpha(footSprite[i], i == 5 ? 1 : 0);
            }
        }
        else
        {
            // 初始化：只显示第一张图片，其他隐藏
            for (int i = 0; i < footSprite.Length; i++)
            {
                SetImageAlpha(footSprite[i], i == 0 ? 1 : 0);
            }
        }
    }

    // 点击当前图片时调用（需要为每张图片的Button组件绑定此方法）
    public void OnImageClick()
    {
        if (!isTransitioning && currentIndex < footSprite.Length - 1)
        {
            StartCoroutine(TransitionToNextImage());
        }
    }

    IEnumerator TransitionToNextImage()
    {
        isTransitioning = true;

        int oldIndex = currentIndex;
        currentIndex++;

        SpriteRenderer currentImage = footSprite[oldIndex];
        SpriteRenderer nextImage = footSprite[currentIndex];

        Debug.Log($"从图片 {oldIndex + 1} 过渡到图片 {currentIndex + 1}");

        // 同时开始当前图片淡出和下一张图片淡入
        Coroutine fadeOut = StartCoroutine(FadeImage(currentImage, 0, fadeDuration,false));
        Coroutine fadeIn = StartCoroutine(FadeImage(nextImage, 1, fadeDuration,true));

        // 等待两个动画都完成
        yield return fadeOut;
        yield return fadeIn;

        isTransitioning = false;

        // 检查是否是最后一张图片
        if (currentIndex >= footSprite.Length - 1)
        {
            Debug.Log("已经是最后一张图片了！");
        }
    }

    // 淡入淡出核心方法
    IEnumerator FadeImage(SpriteRenderer target, float targetAlpha, float duration,bool collder)
    {
        Color startColor = target.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float percentage = Mathf.Clamp01(elapsedTime / duration);
            target.color = Color.Lerp(startColor, endColor, percentage);
            yield return null;
        }

        target.GetComponent<Collider2D>().enabled = collder;
        target.color = endColor;
    }

    // 设置图片透明度
    void SetImageAlpha(SpriteRenderer image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    // 重置到第一张图片（可选功能）
    public void ResetToFirstImage()
    {
        StopAllCoroutines();
        isTransitioning = false;

        for (int i = 0; i < footSprite.Length; i++)
        {
            SetImageAlpha(footSprite[i], i == 0 ? 1 : 0);
        }
        currentIndex = 0;
    }
}
