using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SetPlane : MonoBehaviour
{
    public List<Sprite> Language_SpriteListl;       // 0 英文 1 中文


    [Header("BGM")]
    public Button BGM_Right_BTN;
    public Button BGM_Left_BTN;
    public GameObject BGM_CloseBGM;
    public Transform BGM_Sdlider;

    [Header("SFX")]
    public Button SFX_Right_BTN;
    public Button SFX_Left_BTN;
    public GameObject SFX_CloseSFX;
    public Transform SFX_Sdlider;

    [Header("语言")]
    public Button Language_Right_BTN;
    public Button Language_Left_BTN;
    public Image Language_Img;

    [Header("返回按钮")]
    public Button back_BTN;

    private int BGM_Index;
    private int SFX_Index;


    private int start_X;
    private void Awake()
    {
        // BGM

        BGM_Right_BTN.onClick.AddListener(Right_BGM);
        BGM_Left_BTN.onClick.AddListener(Left_BGM);

        start_X = (int)BGM_Sdlider.localPosition.x;

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
        InitLanguage();

        InitBGM();
        InitSFX();
    }

    #region BGM

    public void InitBGM()
    {

        BGM_Index = PlayerPrefs.GetInt(MusicManager.BGMVolumeKey);

        if (BGM_Index <= 0)
        {
            BGM_CloseBGM.SetActive(true);
            BGM_Left_BTN.interactable = false;
        }
        else
            BGM_CloseBGM.SetActive(false);
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

        if(BGM_Index > 0)
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


    public void ChangeSliderBGM(int iNDEX)
    {
        int variate = iNDEX * 29;
        BGM_Sdlider.localPosition = new Vector3(start_X + variate, BGM_Sdlider.localPosition.y,0);
        MusicManager.Instance.SetVolume_BGM(iNDEX);
    }

    #endregion

    #region SFX

    public void InitSFX()
    {
        SFX_Index = PlayerPrefs.GetInt(MusicManager.SFXVolumeKey);

        if (SFX_Index <= 0)
        {
            SFX_CloseSFX.SetActive(true);
            SFX_Left_BTN.interactable = false;
        }
        else
            SFX_CloseSFX.SetActive(false);
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

    public void ChangeSliderSFX(int iNDEX)
    {
        int variate = iNDEX * 29;
        SFX_Sdlider.localPosition = new Vector3(start_X + variate, SFX_Sdlider.localPosition.y, 0);
        MusicManager.Instance.SetVolume_SFX(iNDEX);
    }

    #endregion


    #region 语言

    /// <summary>
    /// 初始化 语言
    /// </summary>
    public void InitLanguage()
    {
        if (LanguageManager.Instance.CurrentLanguage == SystemLanguage.Chinese)
        {
            Language_Left_BTN.interactable = false;
            Language_Img.sprite = Language_SpriteListl[1];
        }
        else
            Language_Left_BTN.interactable = true;

        if (LanguageManager.Instance.CurrentLanguage == SystemLanguage.English)
        {
            Language_Right_BTN.interactable = false;
            Language_Img.sprite = Language_SpriteListl[0];
        }
        else
            Language_Right_BTN.interactable = true; 

    }



    private void Right_Language()
    {
        LanguageManager.Instance.SetLanguage(SystemLanguage.English);
        Language_Img.sprite = Language_SpriteListl[0];
        if (LanguageManager.Instance.CurrentLanguage == SystemLanguage.English)
        {
            Language_Right_BTN.interactable = false;
            Language_Left_BTN.interactable = true;
        }
        else
            Language_Right_BTN.interactable = true;
    }

    private void Left_Language()
    {
        LanguageManager.Instance.SetLanguage(SystemLanguage.Chinese);
        Language_Img.sprite = Language_SpriteListl[1];
        if (LanguageManager.Instance.CurrentLanguage == SystemLanguage.Chinese)
        {
            Language_Left_BTN.interactable = false;
            Language_Right_BTN.interactable = true;
        }
        else
            Language_Left_BTN.interactable = true;
    }



    private void LoadSavedLanguage()
    {
        string savedLanguage = PlayerPrefs.GetString("SelectedLanguage", SystemLanguage.English.ToString());

        if (System.Enum.TryParse(savedLanguage, out SystemLanguage language))
        {
            LanguageManager.Instance.SetLanguage(language);
        }

    }

    

    #endregion



    private void BackClick()
    {
        throw new NotImplementedException();
    }
}
