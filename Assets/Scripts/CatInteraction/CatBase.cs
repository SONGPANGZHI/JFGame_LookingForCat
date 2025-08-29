using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CatBase : MonoBehaviour
{
    public bool isFound = false;
    public int catID;
    public SkeletonAnimation catAnim;
    public bool loopAnim = true;

    [Header("通用配置")]
    public ParticleSystem foundEffect;
    // 猫找到后的特殊行为字典
    private static Dictionary<int, Action> catSpecialActions;

    [RuntimeInitializeOnLoadMethod]
    private static void InitializeCatActions()
    {
        catSpecialActions = new Dictionary<int, Action>
        {
            // 可以继续添加其他猫的特殊行为
            { 12, () => SevenSeasDeluxe.Instance.StartMove() },
            { 43, ()=> CatBus.Instance.BusMove() },
            
        };
    }


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

    public virtual void OnCatFound()
    {
        if (isFound) return;

        isFound = true;
        SetCatAppearance();

        SpawnEffect();
        //AudioSource.PlayClipAtPoint(foundSound, transform.position);

        ExecuteSpecialAction();
        UpdateUI();
        SaveProgress();
        CheckConditions();
    }

    private void SetCatAppearance()
    {
        if (catAnim != null)
        {
            catAnim.Skeleton.SetColor(Color.gray);
            PlayAnim(0, "Sports", loopAnim);
        }

        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.color = Color.gray;
    }

    private void ExecuteSpecialAction()
    {
        // 执行特殊行为（如果存在）
        if (catSpecialActions.TryGetValue(catID, out Action action))
        {
            action?.Invoke();
        }
    }

    private void UpdateUI()
    {
        UIManager.Instance.ShowCatFoundPopup(this);
    }

    private void SaveProgress()
    {
        GameManager.Instance.progressManager.CatFound(catID);
    }

    private void CheckConditions()
    {
        GameManager.Instance.conditionChecker.CheckConditions();
    }

    public void PlayAnim(int layer, string animName, bool loop)
    {
        catAnim.state.SetAnimation(layer, animName, loop);
    }

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
