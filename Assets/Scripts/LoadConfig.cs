using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadConfig : MonoBehaviour
{
    #region JSON 读取

    public string imagePath = "";
    public List<SkinsItem> skinsItemList = new List<SkinsItem>();
    public Dictionary<string, List<SkinsItem>> marketDic = new Dictionary<string, List<SkinsItem>>();

    private void Start()
    {
        LoadJson();
    }

    void LoadJson()
    {
        StartLoadFurniturePostion("Xiaowu");
    }

    public void StartLoadFurniturePostion(string furniturePostion)
    {
        //数据解析方法
        string jsonContent = Resources.Load<TextAsset>("CatPosConfig/" + furniturePostion + "/" + furniturePostion).text;
        LoadJsonConfig(jsonContent, furniturePostion);
    }

    void LoadJsonConfig(string jsonTxt, string furniturePostion)
    {
        List<SkinsItem> skinsList = new List<SkinsItem>();
        var rootData = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonTxt.ToString());
        // 1.获取image地址
        var skeleton = JsonConvert.DeserializeObject<Dictionary<string, object>>(rootData["skeleton"].ToString());
        if (skeleton.ContainsKey("images"))
        {
            object tempStr;
            skeleton.TryGetValue("images", out tempStr);
            imagePath = tempStr.ToString();
        }

        // 2.获取image的skins信息
        var skins = JsonConvert.DeserializeObject<Dictionary<string, object>>(rootData["skins"].ToString());
        var defaultSkin = JsonConvert.DeserializeObject<Dictionary<string, object>>(skins["default"].ToString());
        int layoutNum = defaultSkin.Count;
        foreach (var skinItem in defaultSkin)
        {
            layoutNum--;
            string nameTemp = skinItem.Key;
            Dictionary<string, object> valuePairs = JsonConvert.DeserializeObject<Dictionary<string, object>>(skinItem.Value.ToString());
            foreach (var skinDataItem in valuePairs)
            {
                var skinData = JsonConvert.DeserializeObject<Dictionary<string, object>>(skinDataItem.Value.ToString());
                var skinsItem = new SkinsItem(skinData, nameTemp, layoutNum, imagePath);
                skinsItemList.Add(skinsItem);
                skinsList.Add(skinsItem);
            }
        }
        marketDic.Add(furniturePostion, skinsList);
    }

    #endregion
}

[Serializable]
public class SkinsItem
{
    public string skinName;
    public string imagePath;
    public float pos_x;
    public float pos_y;
    public float width;
    public float height;
    public bool canClick;
    public int orderLayer;
    public Vector3 localPostion;
    public bool isDefault;
    public SkinsItem(Dictionary<string, object> data, string name, int layoutNum, string _imagePath)
    {
        skinName = name;
        orderLayer = layoutNum;
        imagePath = _imagePath;
        pos_x = data.ContainsKey("x") ? float.Parse(data["x"].ToString()) : 0;
        pos_y = data.ContainsKey("y") ? float.Parse(data["y"].ToString()) : 0;
        width = data.ContainsKey("width") ? float.Parse(data["width"].ToString()) : 0;
        height = data.ContainsKey("height") ? float.Parse(data["height"].ToString()) : 0;
        canClick = (bool)(data.ContainsKey("canClick") ? data["canClick"] : false);
        isDefault = (bool)(data.ContainsKey("canClick") ? true : false);
        Vector3 configVect = new Vector3(pos_x, pos_y, 0);
        //Vector3 resultVect = BaseTools.Instance.MainSceneCamera.ScreenToWorldPoint(configVect);
        //localPostion = new Vector3(resultVect.x, resultVect.y, 0);

    }
}

public class SpriteData
{
    public int spriteID;
    public int spriteLayerID;
    public Vector2 spriteV2;
}
