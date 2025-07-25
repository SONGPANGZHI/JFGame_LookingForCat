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


    public static string BGMKey = "BGMVolume";      // 背景音乐音量键
    public static string SFXKey = "SFXVolume";      // 音效音量键
    public static string ClickKey = "ClickVolume";  // 点击音效音量键
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        // 初始化音频源
        BGM = gameObject.AddComponent<AudioSource>();
        Click = gameObject.AddComponent<AudioSource>();
        SFX = gameObject.AddComponent<AudioSource>();
        // 设置音频源属性
        BGM.loop = true;
        Click.playOnAwake = false;
        SFX.playOnAwake = false;
        // 播放背景音乐
        PlayBGM(0);
    }

    // 播放背景音乐
    public void PlayBGM(int index)
    {
        if (index < 0 || index >= audioClips.Length) return;
        BGM.clip = audioClips[index];
        BGM.Play();
    }

    // 播放点击音效
    public void PlayClickSound()
    {
        if (Click.isPlaying) return; // 如果正在播放则不重复播放
        Click.PlayOneShot(Click.clip);
    }

    // 播放音效
    public void PlaySFX(int index)
    {
        if (index < 0 || index >= audioClips.Length) return;
        SFX.PlayOneShot(audioClips[index]);
    }

    // 播放猫叫声
    public void PlayCatSound(int index)
    {
        if (index < 0 || index >= catClips.Length) return;
        SFX.PlayOneShot(catClips[index]);
    }
}
