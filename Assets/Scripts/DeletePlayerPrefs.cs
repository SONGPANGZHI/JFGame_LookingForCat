//using UnityEngine;
//using UnityEditor;
//using System.IO;
//using System;

//public class DeletePlayerPrefs : MonoBehaviour
//{
//    [MenuItem("PlayerPrefs/Delete All")]
//    public static void DeleteAll()
//    {
//        //清除所有的PlayerPrefs
//        PlayerPrefs.DeleteAll();

//        Debug.Log("==========清除所有PlayerPrefs记录");

//        //目标文件夹
//        string targetDirectory = Application.persistentDataPath;

//        //如果目标文件夹存在
//        if (Directory.Exists(targetDirectory))
//        {
//            //删除
//            Directory.Delete(targetDirectory, true);
//        }

//        Debug.Log("==========清除持久化目录");

//        //目标文件夹
//        string targetDirectory1 = "C:\\Users\\" + Environment.UserName + "\\AppData\\LocalLow\\jfctos\\" + PlayerSettings.companyName + "_" + PlayerSettings.productName + "";

//        //如果目标文件夹存在
//        if (Directory.Exists(targetDirectory1))
//        {
//            //删除
//            Directory.Delete(targetDirectory1, true);
//            Debug.Log("==========清除AB包缓存目录");
//        }
//    }
//}