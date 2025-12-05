using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabDisplacement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform crab;
    [SerializeField] private Transform target;
    [SerializeField] private Sprite crabSprite;

    [Header("Settings")]
    [SerializeField] private float scaleAnimationDuration = 3f;
    [SerializeField] private float moveAnimationDuration = 2f;
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private bool _isMoving;
    private Coroutine _currentCoroutine;

    /// <summary>
    /// 点击螃蟹洞
    /// </summary>
    public void ClickCrabhole()
    {
        if (_isMoving) return;

        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = StartCoroutine(CrabAnimationSequence());
    }

    /// <summary>
    /// 螃蟹动画序列：先改变大小，然后移动到目标位置
    /// </summary>
    private IEnumerator CrabAnimationSequence()
    {
        _isMoving = true;
        CloseCrab();

        yield return CrabChangeSize();
        yield return MoveCrabToTarget();

        _isMoving = false;
        _currentCoroutine = null;

    }

    /// <summary>
    /// 改变螃蟹大小
    /// </summary>
    private IEnumerator CrabChangeSize()
    {
        yield return AnimateTransform(
            startScale: Vector3.zero,
            endScale: Vector3.one,
            startPosition: crab.position,
            animationDuration: scaleAnimationDuration,
            animationCurve: scaleCurve
        );
    }

    /// <summary>
    /// 移动螃蟹到目标位置
    /// </summary>
    private IEnumerator MoveCrabToTarget()
    {
        yield return AnimateTransform(
            startScale: crab.localScale,
            endScale: crab.localScale,
            startPosition: crab.position,
            endPosition: target.position,
            animationDuration: moveAnimationDuration,
            animationCurve: moveCurve
        );

    }


    /// <summary>
    /// 通用的变换动画方法
    /// </summary>
    private IEnumerator AnimateTransform(Vector3 startScale, Vector3 endScale,
                                       Vector3 startPosition, Vector3? endPosition = null,
                                       float animationDuration = 1f,
                                       AnimationCurve animationCurve = null)
    {
        float elapsedTime = 0f;
        Vector3 finalPosition = endPosition ?? startPosition;
        AnimationCurve curve = animationCurve ?? AnimationCurve.Linear(0, 0, 1, 1);

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / animationDuration);
            float evaluatedProgress = curve.Evaluate(progress);

            crab.localScale = Vector3.Lerp(startScale, endScale, evaluatedProgress);
            crab.position = Vector3.Lerp(startPosition, finalPosition, evaluatedProgress);

            yield return null;
        }

        // 确保最终值准确
        crab.localScale = endScale;
        crab.position = finalPosition;
    }

    /// <summary>
    /// 强制停止当前动画并立即完成
    /// </summary>
    public void ForceCompleteAnimation()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }

        crab.localScale = Vector3.one;
        crab.position = target.position;
        _isMoving = false;
    }

    public void CloseCrab()
    {
        transform.GetComponent<Collider2D>().enabled = false;
        transform.GetComponent<SpriteRenderer>().sprite = crabSprite;
    }
}
