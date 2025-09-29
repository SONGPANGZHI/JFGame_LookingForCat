using TMPro;
using UnityEngine;

public class OverheadText : MonoBehaviour
{
    [Header("UI References")]
    public Canvas canvas;
    public TMP_Text textComponent; // 或使用 TextMeshProUGUI

    [Header("Settings")]
    public Vector3 offset = new Vector3(0, 1.5f, 0); // 文本在角色头顶的偏移量
    public string displayText = "角色名称";
    public bool alwaysFaceCamera = true;

    private Transform target; // 跟随的目标（角色）
    private Camera mainCamera;

    void Start()
    {
        // 获取角色Transform
        target = transform;

        // 获取主相机
        mainCamera = Camera.main;

        // 如果没有手动指定UI组件，尝试自动获取
        if (canvas == null)
            canvas = GetComponentInChildren<Canvas>();

        if (textComponent == null)
            textComponent = GetComponentInChildren<TMP_Text>();

        // 设置初始文本
        if (textComponent != null)
            textComponent.text = displayText;
    }

    void Update()
    {
        if (target == null || canvas == null) return;

        // 更新UI位置，使其跟随角色
        UpdateUIPosition();

        // 如果需要，让文本始终面向相机
        if (alwaysFaceCamera && mainCamera != null)
        {
            canvas.transform.rotation = mainCamera.transform.rotation;
        }
    }

    void UpdateUIPosition()
    {
        // 将角色的世界坐标转换为屏幕坐标
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(target.position + offset);

        if (canvas != null)
        {
            canvas.transform.position = screenPosition;
        }
    }

    // 公共方法：更新显示的文本
    public void SetText(string newText)
    {
        displayText = newText;
        if (textComponent != null)
            textComponent.text = newText;
    }

    // 公共方法：显示/隐藏文本
    public void SetVisible(bool isVisible)
    {
        if (canvas != null)
            canvas.gameObject.SetActive(isVisible);
    }

    // 公共方法：更改文本颜色
    public void SetTextColor(Color color)
    {
        if (textComponent != null)
            textComponent.color = color;
    }
}
