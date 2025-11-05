using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string textKey;

    private TMP_Text uiText;

    private void Awake()
    {
        uiText = GetComponent<TMP_Text>();

        // 如果没有指定 key，默认用原始文本内容
        if (string.IsNullOrEmpty(textKey))
            textKey = uiText.text;
    }

    private void OnEnable()
    {
        if (LanguageManager.Instance != null)
        {
            LanguageManager.Instance.OnLanguageChanged.AddListener(OnLanguageChanged);
            UpdateText(); // 确保新打开的界面立即更新
        }
    }

    private void OnDisable()
    {
        if (LanguageManager.Instance != null)
            LanguageManager.Instance.OnLanguageChanged.RemoveListener(OnLanguageChanged);
    }

    private void OnLanguageChanged(LanguageType language)
    {
        UpdateText();
    }

    public void UpdateText()
    {
        if (LanguageManager.Instance == null) return;

        string localizedText = LanguageManager.Instance.GetText(textKey);
        uiText.text = localizedText;
    }

    public void SetTextKey(string newKey)
    {
        textKey = newKey;
        UpdateText();
    }
}