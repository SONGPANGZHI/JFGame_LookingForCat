using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance;

    public List<LightController> lights;

    public Text feedbackText; // 用于显示反馈信息

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

    public bool _isPlayLight = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        InitializePuzzle();
    }

    void Update()
    {
        if (!isPuzzleSolved && CheckSolution())
        {
            PuzzleSolved();
        }
    }

    // 初始化谜题
    public void InitializePuzzle()
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

        //UpdateFeedback("尝试点亮所有正确的颜色！");
    }

    // 重置谜题
    public void ResetPuzzle()
    {
        InitializePuzzle();
    }

    // 检查解决方案
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

    // 解谜成功
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

    // 更新反馈文本
    private void UpdateFeedback(string message)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
        }
    }
}
