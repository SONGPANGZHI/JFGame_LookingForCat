using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public Color currentColor;

    public SpriteRenderer lightSprite;         // 灯
    public SpriteRenderer lightBeam;           // 光束

    //private SpriteRenderer lightRend;

    private bool openLightBeam = false;

    // 定义所有可能颜色（包括干扰色）
    public Color[] colorOptions;

    public void SetColor(Color newColor)
    {
        if(openLightBeam)
        {
            lightBeam.gameObject.SetActive(true);
            openLightBeam = false;
        }

        currentColor = newColor;
        lightSprite.color = currentColor;
        lightBeam.color = currentColor;
    }

    public void PointClick()
    {
        openLightBeam = true;

        if (LightManager.isPuzzleSolved)
            return;
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
