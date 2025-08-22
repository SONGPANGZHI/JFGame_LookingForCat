using Spine;
using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 005猫 椰子掉落
/// </summary>
public class CoconutFell : MonoBehaviour
{
    public Transform targetPosition;

    public InteractiveCat interactiveCat;

    public SkeletonAnimation skeletonAnimation;

    private float moveSpeed = 5f; // 移动速度

    private Vector3 startPosition;

    private void Start()
    {

        startPosition = interactiveCat.transform.position;

        bool isCompleted = GameManager.Instance.progressManager.IsCatFound(5);
        if (isCompleted)
        {
            skeletonAnimation.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
            skeletonAnimation.state.SetAnimation(0, "TreeYezi", false);
            interactiveCat.transform.position = targetPosition.position;
        }
    }

    private bool isMoving = false;

  
    // 碎片移动到目标位置
    IEnumerator MoveToTarget()
    {
        isMoving = true;

        float progress = 0f;

        while (progress < 1f)
        {
            progress += Time.deltaTime * moveSpeed;

            // 移动位置
            interactiveCat.transform.position = Vector3.Lerp(startPosition, targetPosition.position, progress);
            yield return null;
        }

        // 确保最终位置和旋转完全匹配
        interactiveCat.transform.position = targetPosition.position;
        isMoving = false;
    }
    

    /// <summary>
    /// 点击椰子
    /// </summary>
    public void ClickCoconut()
    {
        Spine.AnimationState animationState = skeletonAnimation.AnimationState;
        animationState.Complete += OnAnimationComplete;

        TrackEntry trackEntry = skeletonAnimation.AnimationState.SetAnimation(0, "TreeYezi", false);

    }

    // 事件处理方法的示例
    private void OnAnimationComplete(TrackEntry trackEntry)
    {
        string completedAnimationName = trackEntry.Animation.Name;
        Debug.Log($"动画 {completedAnimationName} 播放完成！");

        // 例如：播放完攻击动画后，切换回待机动画
        if (completedAnimationName == "TreeYezi")
        {
            skeletonAnimation.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;

            if (!isMoving)
            {
                StartCoroutine(MoveToTarget());
            }

            interactiveCat.OnObjectInteracted();
        }
    }

}
