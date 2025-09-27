using Steamworks;
using UnityEngine;

public class SteamManager : MonoBehaviour
{
    //private static bool initialized = false;

    //void Awake()
    //{
    //    if (initialized) return;
    //    initialized = true;

    //    try
    //    {
    //        // 初始化 Steam API
    //        if (!SteamAPI.Init())
    //        {
    //            Debug.LogError("Steam 初始化失败！");
    //        }
    //        else
    //        {
    //            Debug.Log("Steam 初始化成功，欢迎：" + SteamFriends.GetPersonaName());
    //        }
    //    }
    //    catch (System.DllNotFoundException e)
    //    {
    //        Debug.LogError("steam_api64.dll 未找到！ " + e);
    //    }
    //}

    //void Update()
    //{
    //    if (initialized)
    //    {
    //        // 每帧运行 SteamAPI 回调
    //        SteamAPI.RunCallbacks();
    //    }
    //}

    //void OnApplicationQuit()
    //{
    //    // 退出时关闭 Steam API
    //    SteamAPI.Shutdown();
    //}
}
