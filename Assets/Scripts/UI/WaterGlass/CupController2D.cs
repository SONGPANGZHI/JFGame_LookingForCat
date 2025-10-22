using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CupController2D : MonoBehaviour
{
    [Header("杯子设置")]
    public Image cupImage;
    public Image waterFill;
    public Button pourButton;
    public float pourAngle = 30f;
    public float pourDuration = 1f;

    [Header("水设置")]
    public float waterAmount = 100f;
    public float pourRate = 10f;
    public float minWaterHeight = 0.1f;
    public float maxWaterHeight = 0.8f;

    [Header("目标杯子")]
    public TargetCup2D targetCup;

    private bool isPouring = false;
    private Quaternion originalRotation;
    private float currentWaterAmount;
    private RectTransform waterRectTransform;

    void Start()
    {
        originalRotation = cupImage.rectTransform.rotation;
        waterRectTransform = waterFill.rectTransform;
        currentWaterAmount = waterAmount;
        pourButton.onClick.AddListener(StartPouring);

        UpdateWaterDisplay();
    }

    void Update()
    {
        // 键盘控制备用
        if (Input.GetKeyDown(KeyCode.Space) && !isPouring)
        {
            StartPouring();
        }
    }

    public void StartPouring()
    {
        if (!isPouring && currentWaterAmount > 0)
        {
            StartCoroutine(PourRoutine());
        }
    }

    IEnumerator PourRoutine()
    {
        isPouring = true;

        // 倾斜杯子
        float timer = 0f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, pourAngle);

        while (timer < pourDuration)
        {
            timer += Time.deltaTime;
            cupImage.rectTransform.rotation = Quaternion.Lerp(originalRotation, targetRotation, timer / pourDuration);
            yield return null;
        }

        // 开始倒水
        yield return StartCoroutine(PourWater());

        // 恢复杯子位置
        timer = 0f;
        while (timer < pourDuration)
        {
            timer += Time.deltaTime;
            cupImage.rectTransform.rotation = Quaternion.Lerp(targetRotation, originalRotation, timer / pourDuration);
            yield return null;
        }

        cupImage.rectTransform.rotation = originalRotation;
        isPouring = false;
    }

    IEnumerator PourWater()
    {
        float pourTime = 2f; // 倒水持续时间
        float timer = 0f;

        while (timer < pourTime && currentWaterAmount > 0)
        {
            timer += Time.deltaTime;

            // 减少水量
            float waterDecrease = pourRate * Time.deltaTime;
            currentWaterAmount = Mathf.Max(0, currentWaterAmount - waterDecrease);
            UpdateWaterDisplay();

            // 向目标杯子添加水
            if (targetCup != null)
            {
                targetCup.AddWater(waterDecrease);
            }

            yield return null;
        }
    }

    void UpdateWaterDisplay()
    {
        // 更新水填充显示
        float waterPercentage = currentWaterAmount / waterAmount;
        float waterHeight = Mathf.Lerp(minWaterHeight, maxWaterHeight, waterPercentage);

        // 设置水填充的高度
        waterRectTransform.anchorMin = new Vector2(0.1f, 0.1f);
        waterRectTransform.anchorMax = new Vector2(0.9f, 0.1f + waterHeight * 0.8f);

        // 当水很少时改变颜色提示
        if (waterPercentage < 0.2f)
        {
            waterFill.color = Color.red;
        }
        else
        {
            waterFill.color = new Color(0.2f, 0.4f, 1f, 0.8f);
        }
    }

    public void ResetCup()
    {
        currentWaterAmount = waterAmount;
        UpdateWaterDisplay();
        cupImage.rectTransform.rotation = originalRotation;
        StopAllCoroutines();
        isPouring = false;
    }
}
