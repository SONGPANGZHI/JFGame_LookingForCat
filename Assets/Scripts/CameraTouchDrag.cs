using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTouchDrag : MonoBehaviour
{
    [Header("拖拽设置")]
    public float dragSpeed = 2f;
    public float smoothTime = 0.3f;
    public float edgeSize = 50f;

    [Header("边界限制")]
    public bool useBounds = true;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private Vector3 dragOrigin;
    private Vector3 velocity = Vector3.zero;

    [Header("缩放设置")]
    public float minZoom = 13f;             //最小缩放数
    public float maxZoom = 16f;             //最大缩放数
    public float zoomSpeed = 0.5f;          //缩放速度
    public float edgePinchThreshold = 50f;  //双指最小有效距离

    private Camera targetCamera;            //目标相机
    private float targetZoom;               //目标缩放区域
    private float zoomVelocity;
    private float initialTouchDistance;     //初始触摸距离

    public void Start()
    {
        targetCamera = GetComponent<Camera>();
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        InitializeZoom();
    }
    public void Update()
    {
        HandleDrag();

#if UNITY_EDITOR
        HandleEditorZoom();
#endif
        HandleMobileZoom();
        ApplyZoom();
    }

    //拖拽移动摄像机
    public void HandleDrag()
    {
        // 开始拖拽
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = GetMouseWorldPos();
        }

        // 拖拽中
        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - GetMouseWorldPos();
            Vector3 targetPos = transform.position + difference * dragSpeed;

            // 平滑移动
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }

        // 应用边界限制
        if (useBounds)
        {
            ClampCameraPosition();
        }

        
    }

    //确定摄像机位置
    public void ClampCameraPosition()
    {
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        float clampedX = Mathf.Clamp(transform.position.x, minBounds.x + cameraWidth, maxBounds.x - cameraWidth);
        float clampedY = Mathf.Clamp(transform.position.y, minBounds.y + cameraHeight, maxBounds.y - cameraHeight);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    //获取鼠标所在位置
    public Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }

    //初始化缩放大小
    public void InitializeZoom()
    {
        targetZoom = targetCamera.orthographic
            ? targetCamera.orthographicSize
            : targetCamera.fieldOfView;
    }

    //在编辑器缩放操作
    public void HandleEditorZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            float zoomDelta = scroll * zoomSpeed;
            if (!targetCamera.orthographic) zoomDelta *= 3f; // 透视相机需要更大调整

            targetZoom -= zoomDelta;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }
    }

    //在移动端缩放操作
    public void HandleMobileZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // 计算当前双指距离
            float currentDistance = Vector2.Distance(touch1.position, touch2.position);

            // 忽略过小的双指距离(防止误触)
            if (currentDistance < edgePinchThreshold) return;

            // 双指开始
            if (touch2.phase == TouchPhase.Began)
            {
                initialTouchDistance = currentDistance;
            }

            // 双指移动中
            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float scaleFactor = currentDistance / initialTouchDistance;
                float zoomDelta = (1 - scaleFactor) * zoomSpeed;

                if (targetCamera.orthographic)
                {
                    targetZoom += zoomDelta;
                }
                else
                {
                    targetZoom += zoomDelta * 10f; // 透视相机需要更大调整
                }

                targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
                initialTouchDistance = currentDistance;
            }
        }
    }

    //应用改变摄像机大小
    public void ApplyZoom()
    {
        if (targetCamera.orthographic)
        {
            targetCamera.orthographicSize = Mathf.SmoothDamp(
                targetCamera.orthographicSize,
                targetZoom,
                ref zoomVelocity,
                smoothTime);
        }
        else
        {
            targetCamera.fieldOfView = Mathf.SmoothDamp(
                targetCamera.fieldOfView,
                targetZoom,
                ref zoomVelocity,
                smoothTime);
        }
    }
}

