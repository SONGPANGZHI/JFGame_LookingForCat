using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 交互式猫猫
public class InteractiveCat : CatBase
{
    [Header("交互设置")]
    public GameObject interactiveObject;  // 触发猫猫出现的物体（如尾巴、草丛等）
    public Animator revealAnimator;      // 控制猫猫出现的动画器
    public string appearAnimation = "CatAppear";
    public string hideAnimation = "CatHide";

    [Header("点击设置")]
    public float clickCooldown = 0.5f;   // 防止连续误点击

    public bool isRevealed = false;     // 猫猫是否已显示
    private float lastClickTime;

    // 交互物体上的组件
    public class InteractivePart : MonoBehaviour
    {
        public InteractiveCat parentCat;

        public void OnInteracted()
        {
            parentCat.OnObjectInteracted();
        }
    }

    private void Start()
    {
        Initialize();
        SetupInteractiveObject();

        //判断是否解锁猫猫
        if (GameManager.Instance.progressManager.IsCatFound(catID))
        {
            isFound = true;
            SetCatVisible(true); // 如果已找到，直接显示猫猫
        }
        else
        {
            HideCat(); // 如果未找到，隐藏猫猫
        }


        // 初始状态
        //SetCatVisible(false); // 初始隐藏猫猫
    }

    private void SetupInteractiveObject()
    {
        // 确保交互物体有碰撞体
        if (interactiveObject.GetComponent<Collider2D>() == null)
        {
            interactiveObject.AddComponent<BoxCollider2D>();
        }

        // 添加交互脚本
        var interactScript = interactiveObject.AddComponent<InteractivePart>();
        interactScript.parentCat = this;      
    }

    // 点击交互物体时调用
    public void OnObjectInteracted()
    {
        if (!isRevealed && !isFound)
        {
            //关闭显示

            RevealCat();
        }
    }

    // 显示猫猫
    private void RevealCat()
    {
        isRevealed = true;

        // 播放出现动画
        if (revealAnimator != null)
        {
            revealAnimator.Play(appearAnimation);
        }

        // 直接设置可见
        SetCatVisible(true);
    }

    // 隐藏猫猫
    private void HideCat()
    {
        if (!isFound) // 如果还没被找到才隐藏
        {
            isRevealed = false;

            if (revealAnimator != null)
            {
                revealAnimator.Play(hideAnimation);
            }
            else
            {
                SetCatVisible(false);
            }
        }
    }

    // 直接设置猫猫显示/隐藏
    private void SetCatVisible(bool visible)
    {
        GetComponent<SpriteRenderer>().enabled = visible;
        GetComponent<Collider2D>().enabled = visible;
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
