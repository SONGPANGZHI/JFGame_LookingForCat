using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string textKey;

    private TMP_Text uiText;
    private TextMeshProUGUI tmpText;

    private void Awake()
    {
        uiText = GetComponent<TMP_Text>();
        tmpText = GetComponent<TextMeshProUGUI>();

        if (string.IsNullOrEmpty(textKey) && uiText != null)
        {
            textKey = uiText.text;
        }

        UpdateText();
    }

    private void OnEnable()
    {
        LanguageManager.Instance.OnLanguageChanged.AddListener(OnLanguageChanged);
    }

    private void OnDisable()
    {
        if (LanguageManager.Instance != null)
            LanguageManager.Instance.OnLanguageChanged.RemoveListener(OnLanguageChanged);
    }

    private void OnLanguageChanged(SystemLanguage language)
    {
        UpdateText();
    }

    private void UpdateText()
    {
        string localizedText = LanguageManager.Instance.GetText(textKey);

        if (uiText != null)
            uiText.text = localizedText;

        if (tmpText != null)
            tmpText.text = localizedText;
    }

    public void SetTextKey(string newKey)
    {
        textKey = newKey;
        UpdateText();
    }
}