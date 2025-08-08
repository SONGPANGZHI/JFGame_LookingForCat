using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CatBase : MonoBehaviour
{
    public int catID;
    public CatType catType;
    public bool isFound;
    public SkeletonAnimation catAnim;


    public Sprite foundSprite;

    [Header("通用配置")]
    public Animation foundEffect;

    public virtual void Initialize()
    {
        isFound = false;
        GameManager.Instance.catDatabase.RegisterCat(this);
    }


    //封装的播放动画函数
    void PlayAnim(int index, string name, bool b)
    {
        catAnim.state.SetAnimation(index, name, b);
    }


    //
    public virtual void OnCatFound()
    {
        if (isFound) return;

        isFound = true;
        if(catAnim != null)
            PlayAnim(0,"2-Cat",false);
        //foundEffect.Play();
        //AudioSource.PlayClipAtPoint(foundSound, transform.position);

        // 更新UI
        UIManager.Instance.ShowCatFoundPopup(this);

        // 保存进度
        GameManager.Instance.progressManager.CatFound(catID);

        // 检查特殊条件
        GameManager.Instance.conditionChecker.CheckConditions();
    }
}
