using UnityEngine;
using UnityEngine.EventSystems;

//直接点击猫猫
public class CatHiddenObject : MonoBehaviour, IPointerDownHandler
{

    public void OnPointerDown(PointerEventData eventData)
    {
        HandleClick();
    }

    //点击逻辑 用协程播放动画
    public void HandleClick()
    {
        Debug.Log(this.gameObject.name);
    }


    //播放动画
    public void PlayAnim()
    { 
    
    }

    //移除网格 不能点击
    public void RemoveCollider()
    { 
    
    }

}
