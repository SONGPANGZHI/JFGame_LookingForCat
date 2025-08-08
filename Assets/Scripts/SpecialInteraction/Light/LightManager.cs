using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance;

    [Header("七彩灯光")]

    public List<LightController> lights;

    public Color[] correctColors = new Color[]
    {
        Color.red,
        new Color(1f, 0.5f, 0f), // 橙色
        Color.yellow,
        Color.green,
        Color.cyan,
        Color.blue,
        new Color(0.5f, 0f, 0.5f) // 紫色
    };

    public static bool isPuzzleSolved = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        InitializeLight();
    }

    void Update()
    {
        if (!isPuzzleSolved && CheckSolution())
        {
            PuzzleSolved();
        }
    }


    #region 七彩灯光事件

    // 初始化灯光状态
    public void InitializeLight()
    {
        isPuzzleSolved = false;

        // 随机设置一些灯的初始状态（包括干扰色）
        foreach (LightController light in lights)
        {
            // 30%概率设置随机颜色，70%概率保持黑色
            if (Random.Range(0f, 1f) < 0.3f)
            {
                light.SetRandomColor();
            }
            else
            {
                light.SetColor(Color.black);
            }
        }
    }

    // 检查灯光是否匹配
    private bool CheckSolution()
    {
        for (int i = 0; i < lights.Count; i++)
        {
            if (lights[i].currentColor != correctColors[i])
            {
                return false;
            }
        }
        return true;
    }

    // 解锁成功
    public void PuzzleSolved()
    {
        isPuzzleSolved = true;
        //UpdateFeedback("恭喜！解谜成功！");
        Debug.Log("恭喜！解谜成功！");
        // 所有灯闪烁庆祝
        StartCoroutine(CelebrationEffect());
    }

    // 庆祝效果 - 灯闪烁
    private IEnumerator CelebrationEffect()
    {
        float duration = 3f;
        float interval = 0.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            foreach (LightController light in lights)
            {
                light.SetColor(Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f));
            }
            yield return new WaitForSeconds(interval);
            elapsed += interval;

            // 恢复正确颜色
            for (int i = 0; i < lights.Count; i++)
            {
                lights[i].SetColor(correctColors[i]);
            }
            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }

        Debug.Log("庆祝效果结束，所有灯恢复正确颜色。");
    }

    #endregion


    #region 打地鼠事件

    #endregion

    #region 游戏拼图事件

    #endregion

    #region 家具组装事件

    #endregion

    #region 数字顺序事件

    #endregion





   

 
}
