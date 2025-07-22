using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//打地鼠游戏
public class WhackAMole : MonoBehaviour
{
    public int recordCount = 0;             // 记录打地鼠的次数 本次游戏中

    public List<Transform> jerryPos;        // 地鼠出现的点
    public GameObject jerryOBJ;             // 地鼠预制体
    public GameObject catOBJ;               // 猫猫预制体


    private void Awake()
    {
        jerryOBJ.transform.SetParent(GetJerryPos(), false);
        catOBJ.transform.SetParent(GetJerryPos(), false);
    }

    // 随机位置
    public Transform GetJerryPos()
    {
        if (jerryPos.Count == 0) return null;
        int index = Random.Range(0, jerryPos.Count);
        return jerryPos[index];
    }

    // 点击老鼠
    public void OnPointerClick()
    {
        // 处理点击事件
        if (recordCount >= 5)
        {
            jerryOBJ.SetActive(false);
            catOBJ.SetActive(true);
            Debug.Log("已达到打地鼠次数上限");
            return;
        }
        else
        {
            recordCount += 1;
            jerryOBJ.transform.SetParent(GetJerryPos(), false);
        }

        Debug.Log($"点击了老鼠，当前次数: {recordCount}");
        // 这里可以添加更多的逻辑，比如更新UI，播放音效等
        // 例如：UIManager.Instance.UpdateProgressUI();
    }

}
