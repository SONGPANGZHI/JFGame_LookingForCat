using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance { get; private set; }

    [SerializeField] private LanguageData languageData;
    [SerializeField] private LanguageType defaultLanguage = LanguageType.Chinese;

    public LanguageType CurrentLanguage { get; private set; }
    public UnityEvent<LanguageType> OnLanguageChanged { get; private set; } = new UnityEvent<LanguageType>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            string savedLang = PlayerPrefs.GetString("SelectedLanguage", defaultLanguage.ToString());
            if (Enum.TryParse(savedLang, out LanguageType lang))
                SetLanguage(lang, false); // 不触发事件，也不刷新UI
            else
                SetLanguage(defaultLanguage, false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 设置语言
    /// </summary>
    public void SetLanguage(LanguageType language, bool invokeEvent = true)
    {
        CurrentLanguage = language;
        PlayerPrefs.SetString("SelectedLanguage", language.ToString());
        PlayerPrefs.Save();

        if (invokeEvent)
            OnLanguageChanged.Invoke(language);

    }

    /// <summary>
    /// 获取文本
    /// </summary>
    public string GetText(string key)
    {
        if (languageData == null)
        {
            Debug.LogError("LanguageData is not assigned!");
            return key;
        }

        return languageData.GetText(CurrentLanguage, key);
    }

    /// <summary>
    /// 主动刷新所有 LocalizedText
    /// </summary>
    public void RefreshAllLocalizedText()
    {
        var localizedTexts = FindObjectsOfType<LocalizedText>(true);
        foreach (var localized in localizedTexts)
        {
            localized.UpdateText();
        }
    }
}

public enum LanguageType
{
    Chinese,     // 中文
    English,     // 英语
    French,      // 法语
    German,      // 德语
    Japanese,    // 日语
    RU,          // 俄语
    Korean,      // 韩语
    Portuguese,  // 葡萄牙语
    Spanish      // 西班牙语
}