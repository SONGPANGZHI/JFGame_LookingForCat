using UnityEngine;
using UnityEngine.EventSystems;

//简单猫猫藏物 
public class SimpleInteractiveHideCat : MonoBehaviour, IPointerDownHandler
{
    public GameObject hideCatObj;

    #region 点击逻辑

    public void OnPointerDown(PointerEventData eventData)
    {
        HandleClick();
    }

    //点击逻辑 用协程播放动画
    public void HandleClick()
    {
        Debug.Log(this.gameObject.name);
    }

    #endregion

    //播放交互动画
    public void PlayAnim()
    { 
    
    }

    //添加刚体
    public void AddCollider()
    { 
    
    }

    //点击猫猫
    public void ClickCat()
    { 
    
    }
}
