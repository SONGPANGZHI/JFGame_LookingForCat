using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 输入管理器
public class InputManager : MonoBehaviour
{
    [Header("点击设置")]
    [SerializeField] private float touchRadius = 0.5f; // 世界单位的点击检测半径
    [SerializeField] private LayerMask catLayerMask;   // 猫猫所在的层级

    private Camera mainCamera;
    private Vector2 touchStartPos;
    private bool isDragging = false;
    private const float dragThreshold = 20f; // 拖拽判定的像素阈值

    // UI状态控制
    private bool isUIOpen = false;
    public void Initialize()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // 如果UI打开，不处理任何输入
        if (isUIOpen) return;

        ProcessTouchInput();

        // 编辑器测试
#if UNITY_EDITOR
        ProcessMouseInput();
#endif
    }

    // 设置UI状态
    public void SetUIOpenState(bool isOpen)
    {
        isUIOpen = isOpen;
        Debug.Log($"UI状态: {(isOpen ? "打开" : "关闭")}");
    }

    //点击状态下的处理
    private void ProcessTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    isDragging = false;
                    break;

                case TouchPhase.Moved:
                    if (Vector2.Distance(touch.position, touchStartPos) > dragThreshold)
                    {
                        isDragging = true;
                    }
                    break;

                case TouchPhase.Ended:
                    if (!isDragging)
                    {
                        CheckForCatTap(touch.position);
                    }
                    isDragging = false;
                    break;
            }
        }
    }

    private void ProcessMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (Vector2.Distance(Input.mousePosition, touchStartPos) <= dragThreshold)
            {
                CheckForCatTap(Input.mousePosition);
            }
        }
    }

    //检查点击是否在猫猫上
    private void CheckForCatTap(Vector2 screenPosition)
    {
        Vector2 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

        // 只检测特定图层的碰撞体
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            worldPosition,
            touchRadius,
            catLayerMask
        );

        if (hits.Length == 0) return; // 没有点击到任何猫猫相关物体

        // 按优先级处理点击
        foreach (Collider2D hit in hits)
        {
            // 检查交互式猫猫
            var interactiveCat = hit.GetComponent<InteractiveCat>();
            if (interactiveCat != null && interactiveCat.isRevealed)
            {
                interactiveCat.OnCatClicked();
                return;
            }

            // 1. 检查是否是交互部件
            var interactivePart = hit.GetComponent<InteractiveCat.InteractivePart>();
            if (interactivePart != null)
            {
                Debug.Log($"检测到交互部件: {hit.name}", hit.gameObject);
                interactivePart.OnInteracted();
                return;
            }


            // 2. 检查是否是可见猫猫
            var visibleCat = hit.GetComponent<VisibleCat>();
            if (visibleCat != null && !visibleCat.isFound)
            {
                visibleCat.OnTapped();
                return;
            }

            // 3. 检查是否是条件猫猫
            var conditionalCat = hit.GetComponent<ConditionalCat>();
            if (conditionalCat != null && !conditionalCat.isFound && !conditionalCat.IsHidden())
            {
                conditionalCat.OnTapped();
                return;
            }
        }
    }
}
