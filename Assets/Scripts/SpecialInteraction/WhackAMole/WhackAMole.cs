using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

//打地鼠游戏
public class WhackAMole : MonoBehaviour
{
    public int recordCount = 0;             // 记录打地鼠的次数 本次游戏中

    public List<Transform> jerryPos;        // 地鼠出现的点
    public GameObject jerryOBJ;             // 地鼠预制体
    public VisibleCat catOBJ;               // 猫猫预制体
    public GameObject hammerOBJ;            // 锤子预制体

    public static bool isPlaying = false;   // 是否正在玩打地鼠游戏

   
    private void Start()
    {
        CheckIDCat();
    }

    // 判断是否存在ID
    public void CheckIDCat()
    {
        bool isCompleted = GameManager.Instance.progressManager.IsCatFound(catOBJ.catID);

        if (isCompleted)
        {
            isPlaying = false;
            jerryOBJ.SetActive(false);
            hammerOBJ.SetActive(false);
            catOBJ.GetComponent<MeshRenderer>().enabled = true;
            catOBJ.GetComponent<SkeletonAnimation>().enabled = true;
            catOBJ.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "Sports", true);

        }
        else
        {
            jerryOBJ.transform.SetParent(GetJerryPos(), false);
            catOBJ.transform.SetParent(GetJerryPos(), false);
        }

    }


    private void Update()
    {
        if (isPlaying)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f; // 设置一个合适的Z轴距离
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            hammerOBJ.transform.position = worldPos;
        }
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
        if (recordCount > 5)
            return;

        if (isPlaying)
        {
            hammerOBJ.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "ChuiziAd", false);
            hammerOBJ.GetComponent<SkeletonAnimation>().AnimationState.Complete += AnimationState_Complete;
        }
    }

    //点击锤子
    public void ClickHammer()
    {
        isPlaying = true;
        Debug.Log("获得锤子"); 
    }

    private void AnimationState_Complete(TrackEntry trackEntry)
    {
        // 移除监听器，避免重复调用
        hammerOBJ.GetComponent<SkeletonAnimation>().AnimationState.Complete -= AnimationState_Complete;

        // 处理点击事件
        if (recordCount == 4)
        {
            jerryOBJ.SetActive(false);
            catOBJ.GetComponent<MeshRenderer>().enabled = true;
            catOBJ.GetComponent<SkeletonAnimation>().enabled = true;
            catOBJ.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "Sports", true);
            catOBJ.GetComponent<PolygonCollider2D>().enabled = true;
            Debug.Log("已达到打地鼠次数上限");
            hammerOBJ.SetActive(false);
            return;
        }
        else
        {
            recordCount += 1;
            jerryOBJ.transform.SetParent(GetJerryPos(), false);
        }

        Debug.Log($"点击了老鼠，当前次数: {recordCount}");
    }
}
