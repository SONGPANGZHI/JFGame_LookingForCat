using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LanguageData", menuName = "Language System/Language Data")]
public class LanguageData : ScriptableObject
{
    [System.Serializable]
    public class LanguageItem
    {
        public string key;
        public string value;
    }

    [System.Serializable]
    public class Language
    {
        public SystemLanguage language;
        public List<LanguageItem> items = new List<LanguageItem>();
    }

    public List<Language> languages = new List<Language>();

    public string GetText(SystemLanguage lang, string key)
    {
        foreach (var language in languages)
        {
            if (language.language == lang)
            {
                foreach (var item in language.items)
                {
                    if (item.key == key)
                        return item.value;
                }
            }
        }
        return $"[{key}]"; // 如果找不到对应文本，返回键名
    }
}
