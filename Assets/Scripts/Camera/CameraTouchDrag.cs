using UnityEngine;

public class CameraTouchDrag : MonoBehaviour
{

    [Header("拖拽设置")]
    public float dragSpeed = 2f;
    public float smoothTime = 0.3f;

    [Header("边界限制")]
    public bool useBounds = true;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    [Header("缩放设置")]
    public float minZoom = 13f;
    public float maxZoom = 16f;
    public float zoomSpeed = 0.5f;
    public float edgePinchThreshold = 50f;

    private Camera targetCamera;
    private Vector3 dragOrigin;
    private Vector3 velocity = Vector3.zero;
    private bool isDragging = false;

    // 缩放相关变量
    private float targetZoom;
    private float zoomVelocity;
    private float initialTouchDistance;

    void Start()
    {
        targetCamera = GetComponent<Camera>();
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
            if (targetCamera == null)
            {
                Debug.LogError("CameraTouchDrag: 未找到相机组件！");
                return;
            }
        }

        InitializeCamera();
    }

    void Update()
    {
        if (targetCamera == null) return;

        HandleAllInput();
        ApplySmoothMovement();
        ApplyZoom();
    }

    /// <summary>
    /// 初始化相机设置
    /// </summary>
    private void InitializeCamera()
    {
        // 初始化缩放值
        if (targetCamera.orthographic)
        {
            targetZoom = targetCamera.orthographicSize;
        }
        else
        {
            targetZoom = targetCamera.fieldOfView;
        }

        // 确保初始值在合理范围内
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
    }

    /// <summary>
    /// 统一处理所有输入
    /// </summary>
    private void HandleAllInput()
    {
        // 优先处理触摸输入（移动端）
        if (Input.touchCount > 0)
        {
            HandleTouchInput();
        }
        // PC端输入处理
        else
        {
            HandleMouseInput();
            HandlePcZoomInput(); // 修复PC端缩放问题
        }
    }

    #region 鼠标/触摸输入处理

    /// <summary>
    /// 处理触摸输入（移动端）
    /// </summary>
    private void HandleTouchInput()
    {
        switch (Input.touchCount)
        {
            case 1:
                HandleTouchDrag();
                break;
            case 2:
                HandleTouchZoom();
                break;
        }
    }

    /// <summary>
    /// 处理鼠标输入（PC端）
    /// </summary>
    private void HandleMouseInput()
    {
        // 开始拖拽
        if (Input.GetMouseButtonDown(0))
        {
            StartDrag(GetMouseWorldPos());
        }

        // 拖拽中
        if (Input.GetMouseButton(0) && isDragging)
        {
            ContinueDrag(GetMouseWorldPos());
        }

        // 结束拖拽
        if (Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }
    }

    #endregion

    #region 拖拽逻辑

    private void StartDrag(Vector3 worldPos)
    {
        dragOrigin = worldPos;
        isDragging = true;
        velocity = Vector3.zero;
    }

    private void ContinueDrag(Vector3 currentWorldPos)
    {
        Vector3 difference = dragOrigin - currentWorldPos;
        Vector3 targetPos = transform.position + difference * dragSpeed;

        // 使用SmoothDamp进行平滑移动
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        // 更新拖拽起点
        dragOrigin = currentWorldPos;
    }

    private void EndDrag()
    {
        isDragging = false;
    }

    /// <summary>
    /// 处理触摸拖拽
    /// </summary>
    private void HandleTouchDrag()
    {
        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                StartDrag(GetTouchWorldPos(touch.position));
                break;

            case TouchPhase.Moved:
                if (isDragging)
                {
                    ContinueDrag(GetTouchWorldPos(touch.position));
                }
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                EndDrag();
                break;
        }
    }

    #endregion

    #region 缩放逻辑

    /// <summary>
    /// 处理PC端缩放输入（修复版）
    /// </summary>
    private void HandlePcZoomInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // 只有当滚轮有实际输入时才处理
        if (Mathf.Abs(scroll) > 0.01f)
        {
            float zoomDelta = scroll * zoomSpeed;

            // 根据相机类型调整缩放灵敏度
            if (!targetCamera.orthographic)
            {
                zoomDelta *= 3f; // 透视相机需要更大调整
            }

            targetZoom -= zoomDelta;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        }
    }

    /// <summary>
    /// 处理触摸缩放
    /// </summary>
    private void HandleTouchZoom()
    {
        Touch touch1 = Input.GetTouch(0);
        Touch touch2 = Input.GetTouch(1);

        float currentDistance = Vector2.Distance(touch1.position, touch2.position);

        // 忽略过小的双指距离
        if (currentDistance < edgePinchThreshold) return;

        // 双指开始
        if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
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
                targetZoom += zoomDelta * 3f;
            }

            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
            initialTouchDistance = currentDistance;
        }
    }

    /// <summary>
    /// 应用平滑缩放
    /// </summary>
    private void ApplyZoom()
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

    #endregion

    #region 工具方法

    /// <summary>
    /// 应用平滑移动（分离出来以便更好地控制）
    /// </summary>
    private void ApplySmoothMovement()
    {
        if (useBounds)
        {
            ClampCameraPosition();
        }
    }

    /// <summary>
    /// 限制相机位置在边界内
    /// </summary>
    private void ClampCameraPosition()
    {
        if (!targetCamera.orthographic) return; // 只对正交相机进行边界限制

        float cameraHeight = targetCamera.orthographicSize;
        float cameraWidth = cameraHeight * targetCamera.aspect;

        float clampedX = Mathf.Clamp(transform.position.x, minBounds.x + cameraWidth, maxBounds.x - cameraWidth);
        float clampedY = Mathf.Clamp(transform.position.y, minBounds.y + cameraHeight, maxBounds.y - cameraHeight);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    /// <summary>
    /// 获取鼠标世界坐标
    /// </summary>
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = targetCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }

    /// <summary>
    /// 获取触摸世界坐标
    /// </summary>
    private Vector3 GetTouchWorldPos(Vector2 touchPosition)
    {
        Vector3 worldPos = targetCamera.ScreenToWorldPoint(touchPosition);
        worldPos.z = 0;
        return worldPos;
    }

    /// <summary>
    /// 重置拖拽状态（供外部调用）
    /// </summary>
    public void ResetDragState()
    {
        isDragging = false;
        velocity = Vector3.zero;
    }

    /// <summary>
    /// 设置相机边界（供外部调用）
    /// </summary>
    public void SetCameraBounds(Vector2 newMinBounds, Vector2 newMaxBounds)
    {
        minBounds = newMinBounds;
        maxBounds = newMaxBounds;
    }

    #endregion




    //    [Header("拖拽设置")]
    //    public float dragSpeed = 2f;
    //    public float smoothTime = 0.3f;
    //    public float edgeSize = 50f;

    //    [Header("边界限制")]
    //    public bool useBounds = true;
    //    public Vector2 minBounds;
    //    public Vector2 maxBounds;

    //    private Vector3 dragOrigin;
    //    private Vector3 velocity = Vector3.zero;
    //    private bool isDragging = false;

    //    [Header("缩放设置")]
    //    public float minZoom = 13f;             //最小缩放数
    //    public float maxZoom = 16f;             //最大缩放数
    //    public float zoomSpeed = 0.5f;          //缩放速度
    //    public float edgePinchThreshold = 50f;  //双指最小有效距离

    //    private Camera targetCamera;            //目标相机
    //    private float targetZoom;               //目标缩放区域
    //    private float zoomVelocity;
    //    private float initialTouchDistance;     //初始触摸距离

    //    public void Start()
    //    {
    //        targetCamera = GetComponent<Camera>();
    //        if (targetCamera == null)
    //        {
    //            targetCamera = Camera.main;
    //        }

    //        InitializeZoom();
    //    }

    //    public void Update()
    //    {

    //        HandleDrag();
    //#if UNITY_EDITOR
    //        HandleEditorZoom();
    //#endif
    //        HandleMobileZoom();
    //        ApplyZoom();
    //    }

    //    #region 摄像机移动

    //    //拖拽移动摄像机
    //    public void HandleDrag()
    //    {
    //        // 开始拖拽
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            dragOrigin = GetMouseWorldPos();
    //            isDragging = true;
    //            velocity = Vector3.zero; // 重置速度
    //        }

    //        // 拖拽中
    //        if (Input.GetMouseButton(0) && isDragging)
    //        {
    //            Vector3 currentPos = GetMouseWorldPos();
    //            Vector3 difference = dragOrigin - GetMouseWorldPos();
    //            Vector3 targetPos = transform.position + difference * dragSpeed;

    //            // 平滑移动
    //            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

    //            // 更新拖拽起点，使连续拖拽更流畅
    //            dragOrigin = currentPos;
    //        }

    //        // 结束拖拽
    //        if (Input.GetMouseButtonUp(0))
    //        {
    //            isDragging = false;
    //        }

    //        // 应用边界限制
    //        if (useBounds)
    //        {
    //            ClampCameraPosition();
    //        }
    //    }

    //    //确定摄像机位置
    //    public void ClampCameraPosition()
    //    {
    //        float cameraHeight = Camera.main.orthographicSize;
    //        float cameraWidth = cameraHeight * Camera.main.aspect;

    //        float clampedX = Mathf.Clamp(transform.position.x, minBounds.x + cameraWidth, maxBounds.x - cameraWidth);
    //        float clampedY = Mathf.Clamp(transform.position.y, minBounds.y + cameraHeight, maxBounds.y - cameraHeight);

    //        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    //    }

    //    //获取鼠标所在位置
    //    public Vector3 GetMouseWorldPos()
    //    {
    //        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        mousePos.z = 0;
    //        return mousePos;
    //    }

    //    #endregion

    //    #region 摄像机缩放

    //    //初始化缩放大小
    //    public void InitializeZoom()
    //    {
    //        targetZoom = targetCamera.orthographic
    //            ? targetCamera.orthographicSize
    //            : targetCamera.fieldOfView;
    //    }

    //    //在编辑器缩放操作
    //    public void HandleEditorZoom()
    //    {
    //        float scroll = Input.GetAxis("Mouse ScrollWheel");
    //        if (scroll != 0f)
    //        {
    //            float zoomDelta = scroll * zoomSpeed;
    //            if (!targetCamera.orthographic) zoomDelta *= 3f; // 透视相机需要更大调整

    //            targetZoom -= zoomDelta;
    //            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
    //        }
    //    }

    //    //在移动端缩放操作
    //    public void HandleMobileZoom()
    //    {
    //        if (Input.touchCount == 2)
    //        {
    //            Touch touch1 = Input.GetTouch(0);
    //            Touch touch2 = Input.GetTouch(1);

    //            // 计算当前双指距离
    //            float currentDistance = Vector2.Distance(touch1.position, touch2.position);

    //            // 忽略过小的双指距离(防止误触)
    //            if (currentDistance < edgePinchThreshold) return;

    //            // 双指开始
    //            if (touch2.phase == TouchPhase.Began)
    //            {
    //                initialTouchDistance = currentDistance;
    //            }

    //            // 双指移动中
    //            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
    //            {
    //                float scaleFactor = currentDistance / initialTouchDistance;
    //                float zoomDelta = (1 - scaleFactor) * zoomSpeed;

    //                if (targetCamera.orthographic)
    //                {
    //                    targetZoom += zoomDelta;
    //                }
    //                else
    //                {
    //                    targetZoom += zoomDelta * 10f; // 透视相机需要更大调整
    //                }

    //                targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
    //                initialTouchDistance = currentDistance;
    //            }
    //        }
    //    }

    //    public void ResetDragState()
    //    {
    //        isDragging = false;
    //        velocity = Vector3.zero;
    //    }

    //    //应用改变摄像机大小
    //    public void ApplyZoom()
    //    {
    //        if (targetCamera.orthographic)
    //        {
    //            targetCamera.orthographicSize = Mathf.SmoothDamp(
    //                targetCamera.orthographicSize,
    //                targetZoom,
    //                ref zoomVelocity,
    //                smoothTime);
    //        }
    //        else
    //        {
    //            targetCamera.fieldOfView = Mathf.SmoothDamp(
    //                targetCamera.fieldOfView,
    //                targetZoom,
    //                ref zoomVelocity,
    //                smoothTime);
    //        }
    //    }

    //    #endregion


}

