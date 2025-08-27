using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 游乐园 附近的猫猫 ID_27/28/24/23/22/39
/// </summary>
public class AmusementPark : MonoBehaviour
{
    public GameObject valvaOBJ;             //阀门

    public GameObject childrenSlide;        //滑梯

    public List<GameObject> otherCat;       //其他猫猫 在滑梯上 被水流冲出来的小猫

    [SerializeField] private float fadeDuration = 0.5f; // 渐显持续时间
    [SerializeField] private float delayBetweenCats = 2f; // 猫之间的显示延迟


    /// <summary>
    /// 判断是否打开阀门
    /// </summary>
    IEnumerator JudgeOpenValve()
    {
        yield return new WaitForSeconds(1);
        valvaOBJ.GetComponent<SpriteRenderer>().enabled = true;
    }

    /// <summary>
    /// 打开阀门 调用
    /// </summary>
    public void OpenValva()
    {   
        //阀门可以点击
        StartCoroutine(JudgeOpenValve());
    }


    /// <summary>
    /// 阀门点击 
    /// </summary>
    public void ValvaClick()
    {
        // 滑梯动画
        childrenSlide.SetActive(true);
        // 显示出其他猫猫 逐次显示

        StartCoroutine(GraduallyDisplayOtherCat());
    }

    /// <summary>
    /// 逐渐显示其他猫猫
    /// </summary>
    IEnumerator GraduallyDisplayOtherCat()
    {
        foreach (GameObject cat in otherCat)
        {
            // 开始渐显当前猫猫
            yield return StartCoroutine(FadeInCat(cat));

            // 等待指定时间  显示猫
            yield return new WaitForSeconds(delayBetweenCats);
        }
    }

    IEnumerator FadeInCat(GameObject catSprite)
    {
        float elapsedTime = 0f;
        Color color = catSprite.GetComponent<SpriteRenderer>().color;

        while (elapsedTime < fadeDuration)
        {
            // 计算当前的透明度（从0到1）
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            catSprite.GetComponent<SpriteRenderer>().color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终完全显示
        color.a = 1f;
        catSprite.GetComponent<SpriteRenderer>().color = color;

        catSprite.GetComponent<BoxCollider2D>().enabled = true;
    }

}
