using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//音乐管理
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public AudioSource BGM;             // 背景音乐
    public AudioSource Click;           // 点击音效
    public AudioSource SFX;             // 音效音乐
    public AudioClip[] audioClips;      // 背景、音效、点击列表
    public AudioClip[] catClips;        // 猫猫叫声列表


    public static string BGMKey = "BGMVolume";      // 背景音乐音量键 0为打开 1为关闭
    public static string SFXKey = "SFXVolume";      // 音效音量键
    public static string ClickKey = "ClickVolume";  // 点击音效音量键
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        // 播放背景音乐
        PlayBGM(0);
    }

    //初始化
    public void Initialize()
    {
        // 设置音量
        if(!PlayerPrefs.HasKey(BGMKey))
            PlayerPrefs.SetInt(BGMKey, 0); // 默认打开背景音乐

        if (!PlayerPrefs.HasKey(SFXKey))
            PlayerPrefs.SetInt(SFXKey, 0); // 默认打开音效

        if (!PlayerPrefs.HasKey(ClickKey))
            PlayerPrefs.SetInt(ClickKey, 0); // 默认打开点击音效
    }

    // 播放背景音乐
    public void PlayBGM(int index)
    {
        if (PlayerPrefs.GetInt(BGMKey) == 0)
        {
            if (BGM.isPlaying) return; // 如果正在播放则不重复播放

            BGM.Play();
        }
        else
        {
            BGM.Stop(); // 停止播放
            return; // 如果关闭了背景音乐则不播放
        }
    }

    // 播放点击音效
    public void PlayClickSound()
    {
        if (PlayerPrefs.GetInt(ClickKey) == 0)
        {
            Click.Play(); // 播放点击音效
        }
    }

    // 播放音效
    public void PlaySFX(int index)
    {
        if (PlayerPrefs.GetInt(SFXKey) == 0)
        {
            SFX.clip = catClips[index];
            SFX.Play();
        }
            
    }

    // 播放猫叫声
    public void PlayCatSound(int index)
    {
        if (index < 0 || index >= catClips.Length) return;
        SFX.PlayOneShot(catClips[index]);
    }
}
