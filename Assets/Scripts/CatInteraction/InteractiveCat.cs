using Spine.Unity;
using System.Collections;
using UnityEngine;

// 交互式猫猫
public class InteractiveCat : CatBase
{
    [Header("交互设置")]
    public GameObject interactiveObject;  // 触发猫猫出现的物体（如尾巴、草丛等）
    public bool catAnimatorShow = false;            //特殊情况

    [Header("点击设置")]
    public float clickCooldown = 0.5f;   // 防止连续误点击
    public bool isRevealed = false;     // 猫猫是否已显示
    private float lastClickTime;

    [Header("精灵替换设置")]
    public Sprite replacementSprite;     // 替换后的猫猫精灵
    private Sprite originalSprite;       // 原始猫猫精灵


    [Header("交互模式设置")]
    public InteractionMode interactionMode = InteractionMode.None;

    [Header("交互物体动画设置")]
    public InteractiveObjectAnimationType objectAnimationType = InteractiveObjectAnimationType.None;

    public bool IsObstructionDisplayed = false;
    public Transform objectMoveOffset;                  // 物体移动偏移量
    public float objectMoveDuration = 0.5f;             // 物体移动持续时间
    public string objectAnimationName;     // 物体动画名称
    public bool disableAfterInteraction = true;         // 交互后是否禁用物体

    private bool isMoving = false;
    private float moveSpeed = 0.5f; // 根据持续时间计算速度
    private Vector3 startPosition;
    private Vector3 catStartPosition;
    private Quaternion startRotation;

    // 缓存组件引用
    private SpriteRenderer catSpriteRenderer;
    private Collider2D catCollider;
    private SpriteRenderer interactiveObjectSpriteRenderer;
    private Collider2D interactiveObjectCollider;
    private SkeletonAnimation interactiveObjectAnimator;

    /// <summary>
    /// 交互物体上的组件
    /// </summary>
    public class InteractivePart : MonoBehaviour
    {
        public InteractiveCat parentCat;


        public void OnInteracted()
        {
            if (Time.time - parentCat.lastClickTime < parentCat.clickCooldown) return;
                parentCat.lastClickTime = Time.time;

            parentCat.OnObjectInteracted();
        }
    }

    private void Start()
    {
        // 缓存组件引用
        catSpriteRenderer = GetComponent<SpriteRenderer>();
        catCollider = GetComponent<Collider2D>();

        if (interactiveObject != null)
        {
            interactiveObjectSpriteRenderer = interactiveObject.GetComponent<SpriteRenderer>();
            interactiveObjectCollider = interactiveObject.GetComponent<Collider2D>();
            interactiveObjectAnimator = interactiveObject.GetComponent<SkeletonAnimation>();

            startPosition = interactiveObject.transform.position;
            startRotation = interactiveObject.transform.rotation;
        }

        catStartPosition = transform.position;

        // 保存原始精灵
        if (GetComponent<SpriteRenderer>() != null)
        {
            originalSprite = GetComponent<SpriteRenderer>().sprite;
        }

        Initialize();
        SetupInteractiveObject();


        // 判断是否解锁猫猫
        if (isFound)
        {
            HandleAlreadyFoundState();
        }
        else
        {
            HideCat(); // 如果未找到，隐藏猫猫
        
        }

    }

    /// <summary>
    /// 处理已找到状态
    /// </summary>
    private void HandleAlreadyFoundState()
    {
        SetCatVisible(true);

        if (interactiveObject != null)
        {
            interactiveObject.SetActive(false);
        }

        switch (interactionMode)
        {
            case InteractionMode.ReplaceSprite:
                ReplaceCatSprite(replacementSprite);
                break;
        }

        switch (objectAnimationType)
        {
            case InteractiveObjectAnimationType.PositionMove:
                UnlockObjectPosShow();
                break;
            case InteractiveObjectAnimationType.CatPosMove:
                UnlockCatPosShow();
                break;
            case InteractiveObjectAnimationType.CatAndObstacleMove:
                UnlockCatAndObstaclePosShow();
                break;
        }
    }


    /// <summary>
    /// 设置交互物体激活状态
    /// </summary>
    /// <param name="active"></param>
    private void SetInteractiveObjectActive(bool active)
    {
        if (interactiveObjectCollider != null)
            interactiveObjectCollider.enabled = active;

        if (interactiveObjectSpriteRenderer != null)
            interactiveObjectSpriteRenderer.enabled = active;

    }

    /// <summary>
    /// 交互物体添加碰撞体
    /// </summary>
    private void SetupInteractiveObject()
    {
        if (interactiveObject == null) return;

        // 确保交互物体有碰撞体
        if (interactiveObject == null)
            interactiveObjectCollider = interactiveObject.AddComponent<Collider2D>();

        // 添加交互脚本
        var interactScript = interactiveObject.AddComponent<InteractivePart>();
        interactScript.parentCat = this;

        // 初始状态设置
        SetInteractiveObjectActive(true);
    }

    /// <summary>
    /// 执行交互物体动画
    /// </summary>
    private void PlayInteractiveObjectAnimation()
    {
        if (interactiveObject == null) return;

        switch (objectAnimationType)
        {
            case InteractiveObjectAnimationType.None:
                HandleNoneAnimation();
                break;
            case InteractiveObjectAnimationType.PositionMove:
                StartCoroutine(MoveObjectAnimation());
                break;
            case InteractiveObjectAnimationType.CatPosMove:
                StartCoroutine(MoveCatAnimation());
                break;
            case InteractiveObjectAnimationType.CatAndObstacleMove:
                StartCoroutine(MoveCatAndObstacleAnimation());
                break;
            case InteractiveObjectAnimationType.CustomAnimation:
                PlayCustomAnimation();
                break;
            case InteractiveObjectAnimationType.Both:
                StartCoroutine(PlayBothAnimations());
                break;
        }
    }

    /// <summary>
    /// 处理无动画情况
    /// </summary>
    private void HandleNoneAnimation()
    {
        if (disableAfterInteraction)
        {
            SetInteractiveObjectActive(false);
        }
    }

    /// <summary>
    /// 播放自定义动画
    /// </summary>
    private void PlayCustomAnimation()
    {
        //if (interactiveObjectAnimator != null)
        //{
        //    interactiveObjectAnimator.Play(objectAnimationName);
        //    if (disableAfterInteraction)
        //    {
        //        StartCoroutine(DisableAfterAnimation(interactiveObjectAnimator));
        //    }
        //}
        //else if (disableAfterInteraction)
        //{
        //    SetInteractiveObjectActive(false);
        //}
    }

    /// <summary>
    /// 同时播放移动和动画
    /// </summary>
    private IEnumerator PlayBothAnimations()
    {
        // 启动移动动画
        var moveCoroutine = StartCoroutine(MoveObjectAnimation());

        // 播放自定义动画
        //if (interactiveObjectAnimator != null)
        //{
        //    //interactiveObjectAnimator.Play(objectAnimationName);
        //}

        // 等待移动完成
        yield return moveCoroutine;
    }

    /// <summary>
    /// 移动交互物体动画协程
    /// </summary>
    private IEnumerator MoveObjectAnimation()
    {
        isMoving = true;
        float progress = 0f;

        while (progress < 1f)
        {
            progress += Time.deltaTime * moveSpeed;

            // 移动位置
            interactiveObject.transform.SetPositionAndRotation(
                Vector3.Lerp(startPosition, objectMoveOffset.position, progress),
                Quaternion.Lerp(startRotation, objectMoveOffset.rotation, progress)
            );

            yield return null;
        }

        // 确保最终位置和旋转完全匹配
        interactiveObject.transform.SetPositionAndRotation(objectMoveOffset.position, objectMoveOffset.rotation);
        isMoving = false;

        // 移动完成后禁用
        if (disableAfterInteraction)
        {
            if (interactiveObjectSpriteRenderer != null)
                interactiveObjectSpriteRenderer.enabled = true;

            if (interactiveObjectCollider != null)
                interactiveObjectCollider.enabled = false;
        }
    }

    /// <summary>
    /// 解锁交互物位置显示
    /// </summary>
    public void UnlockObjectPosShow()
    {
        if (interactiveObject != null)
        {
            interactiveObject.SetActive(true);
            interactiveObject.transform.SetPositionAndRotation(objectMoveOffset.position, objectMoveOffset.rotation);
        }
    }


    /// <summary>
    /// 猫猫位移动画
    /// </summary>
    private IEnumerator MoveCatAnimation()
    {
        isMoving = true;
        float progress = 0f;
        Vector3 initialPosition = transform.position;

        while (progress < 1f)
        {
            progress += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(initialPosition, objectMoveOffset.position, progress);
            yield return null;
        }

        transform.position = objectMoveOffset.position;
        isMoving = false;

        // 移动完成后禁用交互物体碰撞体
        if (disableAfterInteraction && interactiveObjectCollider != null)
        {
            interactiveObjectCollider.enabled = false;
        }
    }

    /// <summary>
    /// 解锁猫位置显示
    /// </summary>
    public void UnlockCatPosShow()
    {
        transform.SetPositionAndRotation(objectMoveOffset.position, objectMoveOffset.rotation);
    }

   

    /// <summary>
    /// 猫猫和交互物位移动画
    /// </summary>
    private IEnumerator MoveCatAndObstacleAnimation()
    {
        isMoving = true;
        float progress = 0f;
        Vector3 initialCatPosition = transform.position;
        Vector3 initialObjectPosition = interactiveObject.transform.position;

        while (progress < 1f)
        {
            progress += Time.deltaTime * moveSpeed;

            transform.position = Vector3.Lerp(initialCatPosition, objectMoveOffset.position, progress);
            interactiveObject.transform.position = Vector3.Lerp(initialObjectPosition, objectMoveOffset.position, progress);

            yield return null;
        }

        transform.position = objectMoveOffset.position;
        interactiveObject.transform.position = objectMoveOffset.position;
        isMoving = false;

        // 移动完成后处理
        if (disableAfterInteraction)
        {
            if (catAnim != null)
            {
                catAnim.skeleton.SetToSetupPose();
                catAnim.AnimationState.ClearTracks();
                PlayAnim(0, "Sports", true);
            }
        }
    }

    /// <summary>
    /// 解锁猫和障碍物位置显示
    /// </summary>
    public void UnlockCatAndObstaclePosShow()
    {
        transform.position = objectMoveOffset.position;
        if (interactiveObject != null)
        {
            interactiveObject.transform.position = objectMoveOffset.position;
        }
    }

    /// <summary>
    /// 等待动画完成后禁用物体
    /// </summary>
    private IEnumerator DisableAfterAnimation(Animator animator)
    {
        // 等待动画开始
        yield return null;

        // 等待动画完成
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        SetInteractiveObjectActive(false);
    }


    /// <summary>
    /// 点击交互物体时调用
    /// </summary>
    public void OnObjectInteracted()
    {
        if (!isRevealed && !isFound)
        {
            // 先执行交互物体动画
            PlayInteractiveObjectAnimation();

            // 然后显示猫猫

            RevealCat();
        }
    }

    /// <summary>
    /// 显示猫猫
    /// </summary>
    private void RevealCat()
    {
        isRevealed = true;

        if (!IsObstructionDisplayed && interactiveObject != null) 
            interactiveObject.SetActive(false);

        // 直接设置可见
        SetCatVisible(true);

        switch (interactionMode)
        {
            case InteractionMode.ReplaceSprite:
                ReplaceCatSpriteIfAvailable();
                break;

            case InteractionMode.EnableCollider:
                EnableCatCollider();
                break;

            case InteractionMode.Both:
                ReplaceCatSpriteIfAvailable();
                EnableCatCollider();
                break;
        }
    }

    /// <summary>
    /// 替换猫猫精灵
    /// </summary>
    private void ReplaceCatSpriteIfAvailable()
    {
        if (replacementSprite != null)
        {
            ReplaceCatSprite(replacementSprite);
        }
    }

    /// <summary>
    /// 启用猫猫碰撞体
    /// </summary>
    private void EnableCatCollider()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true;
        }
    }

    /// <summary>
    /// 隐藏猫猫
    /// </summary>
    private void HideCat()
    {
        if (!isFound) // 如果还没被找到才隐藏
        {
            isRevealed = false;

            SetCatVisible(false);

            // 确保交互物体是激活的
            SetInteractiveObjectActive(true);
        }
    }

    /// <summary>
    /// 直接设置猫猫显示/隐藏
    /// </summary>
    /// <param name="visible"></param>
    private void SetCatVisible(bool visible)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = visible;

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = visible;

        if (interactionMode == InteractionMode.EnableCollider)
        {
            if(GetComponent<SpriteRenderer>() != null)
                GetComponent<SpriteRenderer>().enabled = true;
        }

        if (catAnimatorShow)
        {
            transform.GetComponent<MeshRenderer>().enabled = visible;
            transform.GetComponent<SkeletonAnimation>().enabled = visible;
        }

    }

    /// <summary>
    /// 替换猫猫精灵
    /// </summary>
    /// <param name="newSprite"> 新图 </param>
    private void ReplaceCatSprite(Sprite newSprite)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = newSprite;
            //spriteRenderer.color = Color.gray;
        }
    }

    // 点击猫猫时调用（由InputManager检测）
    public void OnCatClicked()
    {
        if (isRevealed && !isFound)
        {
            OnCatFound(); // 调用基类方法
        }
    }
}
