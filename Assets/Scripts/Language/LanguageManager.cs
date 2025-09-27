using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance { get; private set; }

    [SerializeField] private LanguageData languageData;
    [SerializeField] private SystemLanguage defaultLanguage = SystemLanguage.English;

    public SystemLanguage CurrentLanguage { get; private set; }
    public UnityEvent<SystemLanguage> OnLanguageChanged { get; private set; } = new UnityEvent<SystemLanguage>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 设置默认语言
            SetLanguage(defaultLanguage);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLanguage(SystemLanguage language)
    {
        CurrentLanguage = language;
        PlayerPrefs.SetString("SelectedLanguage", language.ToString());
        OnLanguageChanged.Invoke(language);
    }

    public string GetText(string key)
    {
        if (languageData == null)
        {
            Debug.LogError("LanguageData is not assigned!");
            return key;
        }

        return languageData.GetText(CurrentLanguage, key);
    }

    public void ToggleLanguage()
    {
        // 简单切换中英文
        if (CurrentLanguage == SystemLanguage.English)
            SetLanguage(SystemLanguage.Chinese);
        else
            SetLanguage(SystemLanguage.English);
    }
}
