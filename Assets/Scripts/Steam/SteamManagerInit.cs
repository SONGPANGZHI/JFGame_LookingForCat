using Steamworks;
using UnityEngine;

public class SteamManagerInit : MonoBehaviour
{
    private void Awake()
    {
        if (!SteamAPI.Init())
        {
            Debug.LogError("Steam 初始化失败！");
            return;
        }

        Debug.Log("Steam 初始化成功，欢迎：" + SteamFriends.GetPersonaName());
    }

    private void Update()
    {
        SteamAPI.RunCallbacks();
    }

    private void OnApplicationQuit()
    {
        SteamAPI.Shutdown();
    }

}
