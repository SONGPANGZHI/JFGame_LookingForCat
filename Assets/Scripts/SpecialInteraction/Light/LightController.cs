using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public Color currentColor;
    private SpriteRenderer lightRend;

    // 定义所有可能颜色（包括干扰色）
    public Color[] colorOptions = new Color[]
    {
        Color.black,        // 默认关闭状态
        Color.red,
        new Color(1f, 0.5f, 0f), // 橙色
        Color.yellow,
        Color.green,
        Color.cyan,
        Color.blue,
        new Color(0.5f, 0f, 0.5f), // 紫色
        Color.white,        // 干扰色1
        new Color(0.5f, 0.5f, 0.5f), // 干扰色2 - 灰色
        new Color(1f, 0f, 1f),      // 干扰色3 - 品红
        new Color(0f, 0.5f, 0f)      // 干扰色4 - 深绿色
    };

    void Awake()
    {
        lightRend = GetComponent<SpriteRenderer>();
        lightRend.color = currentColor;
    }

    public void SetColor(Color newColor)
    {
        currentColor = newColor;
        lightRend.color = currentColor;
    }

    public void PointClick()
    {
        if (LightManager.isPuzzleSolved)
            LightManager.Instance.PuzzleSolved();
        else
            CycleColor();

    }

    // 循环切换颜色
    private void CycleColor()
    {
        int currentIndex = System.Array.IndexOf(colorOptions, currentColor);
        int nextIndex = (currentIndex + 1) % colorOptions.Length;
        SetColor(colorOptions[nextIndex]);

    }

    // 随机设置一个颜色（包括干扰色）
    public void SetRandomColor()
    {
        int randomIndex = Random.Range(0, colorOptions.Length);
        SetColor(colorOptions[randomIndex]);
    }
}
