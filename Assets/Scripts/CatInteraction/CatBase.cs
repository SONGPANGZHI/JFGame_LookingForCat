using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CatBase : MonoBehaviour
{
    public int catID;
    public CatType catType;
    public bool isFound;
    public SkeletonAnimation catAnim;


    public SpriteRenderer foundSprite;

    [Header("通用配置")]
    public ParticleSystem foundEffect;
    public int layerIndex = 0; // 用于控制渲染顺序

    public virtual void Initialize()
    {
        //判断是否解锁猫猫
        if (GameManager.Instance.progressManager.IsCatFound(catID))
        {
            isFound = true;
        }
        else
        {
            isFound = false;
        }
        
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
        if (catAnim != null)
            PlayAnim(0, "Sports", false);

        foundSprite.color = Color.gray;
        SpawnEffect();
        //AudioSource.PlayClipAtPoint(foundSound, transform.position);

        // 更新UI
        UIManager.Instance.ShowCatFoundPopup(this);

        // 保存进度
        GameManager.Instance.progressManager.CatFound(catID);

        // 检查特殊条件
        GameManager.Instance.conditionChecker.CheckConditions();
    }

    // 生成粒子特效
    public void SpawnEffect()
    {
        if (foundEffect != null)
        {
            ParticleSystem effect = Instantiate(foundEffect, transform);
            
            //effect.layer = layerIndex
            effect.Play();
        }
    }
}
