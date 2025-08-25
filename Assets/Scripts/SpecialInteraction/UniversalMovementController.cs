using System;
using System.Collections;
using UnityEngine;

public class UniversalMovementController : MonoBehaviour
{
    [Header("通用设置")]
    public float moveSpeed = 0.5f;
    public bool isMoving = false;

    /// <summary>
    /// 开始移动（支持所有参数传入）
    /// </summary>
    public void StartMove(GameObject otherCat, Transform moveTrans, Transform targetPosition, Action onComplete = null)
    {
        if (otherCat != null)
            otherCat.SetActive(true);

        if (!isMoving && moveTrans != null && targetPosition != null)
        {
            StartCoroutine(MoveToTarget(otherCat, moveTrans, targetPosition, onComplete));
        }
    }

    /// <summary>
    /// 开始移动（不需要otherCat的情况）
    /// </summary>
    public void StartMove(Transform moveTrans, Transform targetPosition, Action onComplete = null)
    {
        if (!isMoving && moveTrans != null && targetPosition != null)
        {
            StartCoroutine(MoveToTarget(null, moveTrans, targetPosition, onComplete));
        }
    }

    /// <summary>
    /// 移动到目标位置
    /// </summary>
    IEnumerator MoveToTarget(GameObject otherCat, Transform moveTrans, Transform targetPosition, Action onComplete = null)
    {
        isMoving = true;
        float progress = 0f;
        Vector3 startPosition = moveTrans.position;

        // 移动过程
        while (progress < 1f)
        {
            progress += Time.deltaTime * moveSpeed;
            moveTrans.position = Vector3.Lerp(startPosition, targetPosition.position, progress);
            yield return null;
        }

        // 确保最终位置准确
        moveTrans.position = targetPosition.position;

        // 执行回调函数
        onComplete?.Invoke();

        isMoving = false;
    }

    /// <summary>
    /// 带自定义速度的移动
    /// </summary>
    public void StartMoveWithSpeed(Transform moveTrans, Transform targetPosition, float customSpeed, Action onComplete = null)
    {
        if (!isMoving)
        {
            StartCoroutine(MoveToTargetWithSpeed(moveTrans, targetPosition, customSpeed, onComplete));
        }
    }

    IEnumerator MoveToTargetWithSpeed(Transform moveTrans, Transform targetPosition, float customSpeed, Action onComplete = null)
    {
        isMoving = true;
        float progress = 0f;
        Vector3 startPosition = moveTrans.position;

        while (progress < 1f)
        {
            progress += Time.deltaTime * customSpeed;
            moveTrans.position = Vector3.Lerp(startPosition, targetPosition.position, progress);
            yield return null;
        }

        moveTrans.position = targetPosition.position;
        onComplete?.Invoke();
        isMoving = false;
    }
}
