using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetCup2D : MonoBehaviour
{
    [Header("目标杯子设置")]
    public Image targetCupImage;
    public Image targetWaterFill;
    public float targetCapacity = 100f;
    public float perfectRange = 10f; // 完美范围

    [Header("游戏设置")]
    public GameManager2D gameManager;

    private float currentWaterAmount = 0f;
    private RectTransform waterRectTransform;

    void Start()
    {
        waterRectTransform = targetWaterFill.rectTransform;
        UpdateWaterDisplay();
    }

    public void AddWater(float amount)
    {
        currentWaterAmount += amount;
        currentWaterAmount = Mathf.Min(currentWaterAmount, targetCapacity);
        UpdateWaterDisplay();

        // 检查是否达到目标
        if (Mathf.Abs(currentWaterAmount - targetCapacity) <= perfectRange)
        {
            gameManager.PerfectPour();
        }
    }

    void UpdateWaterDisplay()
    {
        // 更新目标杯子水填充显示
        float waterPercentage = currentWaterAmount / targetCapacity;
        float waterHeight = Mathf.Lerp(0.1f, 0.8f, waterPercentage);

        waterRectTransform.anchorMin = new Vector2(0.1f, 0.1f);
        waterRectTransform.anchorMax = new Vector2(0.9f, 0.1f + waterHeight * 0.8f);

        // 根据水量改变颜色
        if (waterPercentage > 0.9f)
        {
            targetWaterFill.color = Color.green; // 接近目标
        }
        else
        {
            targetWaterFill.color = new Color(0.2f, 0.4f, 1f, 0.8f); // 正常蓝色
        }
    }

    public void ResetTarget()
    {
        currentWaterAmount = 0f;
        UpdateWaterDisplay();
    }

    public float GetWaterPercentage()
    {
        return currentWaterAmount / targetCapacity;
    }

    public bool IsPerfect()
    {
        return Mathf.Abs(currentWaterAmount - targetCapacity) <= perfectRange;
    }
}
