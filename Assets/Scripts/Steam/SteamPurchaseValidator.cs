//using Steamworks;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SteamPurchaseValidator : MonoBehaviour
//{
//    [Header("是否在开发阶段跳过购买验证")]
//    public bool skipCheckInEditor = true;

//    void Start()
//    {
//#if UNITY_EDITOR
//        if (skipCheckInEditor)
//        {
//            Debug.Log("开发模式：跳过购买验证（编辑器中）");
//            return;
//        }
//#endif

//        if (!SteamAPI.Init())
//        {
//            Debug.LogWarning("Steam 未初始化，跳过验证。");
//            return;
//        }

//        AppId_t appId = (AppId_t)SteamUtils.GetAppID();
//        bool hasLicense = SteamApps.BIsSubscribedApp(appId);

//        if (hasLicense)
//        {
//            Debug.LogWarning("当前用户未购买游戏！(AppID: " + appId + ")");
//            Debug.Log("当前用户已购买此游戏。");
//        }
//        else
//        {
//            Debug.LogWarning("当前用户未购买游戏！(AppID: " + appId + ")");
//#if !UNITY_EDITOR
//            // 可在正式版中退出或提示购买
//            // Application.Quit();
//#endif
//        }
//    }
//}
