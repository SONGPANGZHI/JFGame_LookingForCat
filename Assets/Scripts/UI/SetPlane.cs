using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SetPlane : MonoBehaviour
{
    public List<Sprite> Language_SpriteList;       // 按LanguageType枚举顺序对应

    [Header("BGM")]
    public Button BGM_Right_BTN;
    public Button BGM_Left_BTN;
    public GameObject BGM_CloseBGM;
    public Transform BGM_Slider;

    [Header("SFX")]
    public Button SFX_Right_BTN;
    public Button SFX_Left_BTN;
    public GameObject SFX_CloseSFX;
    public Transform SFX_Slider;

    [Header("语言")]
    public Button Language_Right_BTN;
    public Button Language_Left_BTN;
    public Image Language_Img;

    [Header("返回按钮")]
    public Button back_BTN;

    private int BGM_Index;
    private int SFX_Index;
    private int currentLanguageIndex;

    private int start_X;

    private void Awake()
    {
        // BGM
        BGM_Right_BTN.onClick.AddListener(Right_BGM);
        BGM_Left_BTN.onClick.AddListener(Left_BGM);

        start_X = (int)BGM_Slider.localPosition.x;

        // SFX
        SFX_Right_BTN.onClick.AddListener(Right_SFX);
        SFX_Left_BTN.onClick.AddListener(Left_SFX);

        // 语言
        Language_Right_BTN.onClick.AddListener(Right_Language);
        Language_Left_BTN.onClick.AddListener(Left_Language);

        // 返回按钮
        //back_BTN.onClick.AddListener(BackClick);
    }

    private void Start()
    {
        LoadSavedLanguage();
        InitBGM();
        InitSFX();
    }

    #region BGM

    public void InitBGM()
    {
        BGM_Index = PlayerPrefs.GetInt(MusicManager.BGMVolumeKey, 5); // 默认值5

        if (BGM_Index <= 0)
        {
            BGM_CloseBGM.SetActive(true);
            BGM_Left_BTN.interactable = false;
        }
        else
        {
            BGM_CloseBGM.SetActive(false);
            BGM_Left_BTN.interactable = true;
        }

        if (BGM_Index >= 10)
            BGM_Right_BTN.interactable = false;
        else
            BGM_Right_BTN.interactable = true;

        ChangeSliderBGM(BGM_Index);
    }

    private void Right_BGM()
    {
        BGM_Left_BTN.interactable = true;
        BGM_Index += 1;

        if (BGM_Index > 0)
            BGM_CloseBGM.SetActive(false);
        if (BGM_Index >= 10)
            BGM_Right_BTN.interactable = false;

        ChangeSliderBGM(BGM_Index);
    }

    private void Left_BGM()
    {
        BGM_Right_BTN.interactable = true;
        BGM_Index -= 1;
        if (BGM_Index <= 0)
        {
            BGM_CloseBGM.SetActive(true);
            BGM_Left_BTN.interactable = false;
        }
        else
            BGM_CloseBGM.SetActive(false);

        ChangeSliderBGM(BGM_Index);
    }

    public void ChangeSliderBGM(int index)
    {
        int variate = index * 29;
        BGM_Slider.localPosition = new Vector3(start_X + variate, BGM_Slider.localPosition.y, 0);
        MusicManager.Instance.SetVolume_BGM(index);
    }

    #endregion

    #region SFX

    public void InitSFX()
    {
        SFX_Index = PlayerPrefs.GetInt(MusicManager.SFXVolumeKey, 5); // 默认值5

        if (SFX_Index <= 0)
        {
            SFX_CloseSFX.SetActive(true);
            SFX_Left_BTN.interactable = false;
        }
        else
        {
            SFX_CloseSFX.SetActive(false);
            SFX_Left_BTN.interactable = true;
        }

        if (SFX_Index >= 10)
            SFX_Right_BTN.interactable = false;
        else
            SFX_Right_BTN.interactable = true;

        ChangeSliderSFX(SFX_Index);
    }

    private void Right_SFX()
    {
        SFX_Left_BTN.interactable = true;
        SFX_Index += 1;

        if (SFX_Index > 0)
            SFX_CloseSFX.SetActive(false);
        if (SFX_Index >= 10)
            SFX_Right_BTN.interactable = false;

        ChangeSliderSFX(SFX_Index);
    }

    private void Left_SFX()
    {
        SFX_Right_BTN.interactable = true;
        SFX_Index -= 1;
        if (SFX_Index <= 0)
        {
            SFX_CloseSFX.SetActive(true);
            SFX_Left_BTN.interactable = false;
        }
        else
            SFX_CloseSFX.SetActive(false);

        ChangeSliderSFX(SFX_Index);
    }

    public void ChangeSliderSFX(int index)
    {
        int variate = index * 29;
        SFX_Slider.localPosition = new Vector3(start_X + variate, SFX_Slider.localPosition.y, 0);
        MusicManager.Instance.SetVolume_SFX(index);
    }

    #endregion

    #region 语言
    /// <summary>
    /// 初始化语言
    /// </summary>
    private void InitLanguage()
    {
        UpdateLanguageDisplay();
        UpdateLanguageButtons();
    }

    private void Right_Language()
    {
        if (currentLanguageIndex < Enum.GetValues(typeof(LanguageType)).Length - 1)
        {
            currentLanguageIndex++;
            ChangeLanguage(currentLanguageIndex);
        }
    }

    private void Left_Language()
    {
        if (currentLanguageIndex > 0)
        {
            currentLanguageIndex--;
            ChangeLanguage(currentLanguageIndex);
        }
    }

    private void ChangeLanguage(int languageIndex)
    {
        LanguageType newLanguage = (LanguageType)languageIndex;
        LanguageManager.Instance.SetLanguage(newLanguage);

        UpdateLanguageDisplay();
        UpdateLanguageButtons();

        // 保存语言设置
        PlayerPrefs.SetString("SelectedLanguage", newLanguage.ToString());
        PlayerPrefs.Save();
    }

    private void UpdateLanguageDisplay()
    {
        if (Language_SpriteList != null && Language_SpriteList.Count > currentLanguageIndex)
        {
            Language_Img.sprite = Language_SpriteList[currentLanguageIndex];
        }
        else
        {
            Debug.LogWarning($"Language sprite list配置错误: 列表长度{Language_SpriteList?.Count}, 需要索引{currentLanguageIndex}");
        }
    }

    private void UpdateLanguageButtons()
    {
        // 更新左右按钮的交互状态
        Language_Left_BTN.interactable = currentLanguageIndex > 0;
        Language_Right_BTN.interactable = currentLanguageIndex < Enum.GetValues(typeof(LanguageType)).Length - 1;
    }

    private void LoadSavedLanguage()
    {
        string savedLanguage = PlayerPrefs.GetString("SelectedLanguage", LanguageType.Chinese.ToString());

        if (System.Enum.TryParse(savedLanguage, out LanguageType language))
        {
            currentLanguageIndex = (int)language;
            // 只设置索引，不重复调用LanguageManager.Instance.SetLanguage
        }
        else
        {
            // 如果解析失败，使用默认语言
            currentLanguageIndex = (int)LanguageType.Chinese;
        }

        // 初始化语言显示
        InitLanguage();

        // 确保LanguageManager使用正确的语言
        LanguageManager.Instance.SetLanguage((LanguageType)currentLanguageIndex);
    }
    #endregion

}
