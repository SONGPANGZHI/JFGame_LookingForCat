using System;
using System.Collections;
using System.Collections.Generic;
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

    private int BGM_Index = 0;
    private int SFX_Index = 0;

    private void Awake()
    {
        //// BGM

        //BGM_Right_BTN.onClick.AddListener(Right_BGM);
        //BGM_Left_BTN.onClick.AddListener(Left_BGM);

        //// SFX
        //SFX_Right_BTN.onClick.AddListener(Right_SFX);
        //SFX_Left_BTN.onClick.AddListener(Left_SFX);

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
    }

    #region BGM
    private void Right_BGM()
    {
        
    }

    private void Left_BGM()
    {
       
    }

    #endregion

    #region SFX
    private void Right_SFX()
    {
        throw new NotImplementedException();
    }

    private void Left_SFX()
    {
        throw new NotImplementedException();
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
