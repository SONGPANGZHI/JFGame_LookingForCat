using Spine;
using Spine.Unity;
using System.Collections;
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

    // 新增变量
    private float hammerRange = 5f;          // 锤子移动范围半径
    private Vector3 hammerStartPosition;    // 锤子初始位置
    private bool isHammerReturning = false; // 锤子是否正在返回中

    private void Start()
    {
        // 保存锤子初始位置
        hammerStartPosition = hammerOBJ.transform.position;
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

            // 检查锤子是否超出范围
            if (IsHammerOutOfRange(worldPos) && !isHammerReturning)
            {
                StartCoroutine(ReturnHammerToStart());
            }
            else if (!isHammerReturning)
            {
                hammerOBJ.transform.position = worldPos;
            }
        }
    }

    // 检查锤子是否超出范围
    private bool IsHammerOutOfRange(Vector3 currentPosition)
    {
        float distance = Vector3.Distance(hammerStartPosition, currentPosition);
        return distance > hammerRange;
    }

    // 锤子返回初始位置的协程
    private IEnumerator ReturnHammerToStart()
    {
        //isHammerReturning = true;
        //isPlaying = false;

        //float returnDuration = 0.5f; // 返回动画持续时间
        //float elapsedTime = 0f;
        //Vector3 startPos = hammerOBJ.transform.position;

        //// 可选：添加返回动画效果
        //hammerOBJ.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "Return", false);

        //while (elapsedTime < returnDuration)
        //{
        //    hammerOBJ.transform.position = Vector3.Lerp(startPos, hammerStartPosition, elapsedTime / returnDuration);
        //    elapsedTime += Time.deltaTime;
        //    yield return null;
        //}

        yield return null;
        // 确保最终位置准确
        hammerOBJ.transform.position = hammerStartPosition;
        isHammerReturning = true;
        isPlaying = false;
        // 重置锤子动画
        hammerOBJ.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "Stay", false);

        Debug.Log("锤子已返回初始位置");

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
        if (recordCount > 5 || isHammerReturning)
            return;

        if (isPlaying)
        {
            hammerOBJ.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "ChuiziAd", false);
            hammerOBJ.GetComponent<SkeletonAnimation>().AnimationState.Complete += AnimationState_Complete;
        }
    }

    // 点击锤子
    public void ClickHammer()
    {
        isPlaying = true;
        isHammerReturning = false;
        Debug.Log("获得锤子");
    }

    private void AnimationState_Complete(TrackEntry trackEntry)
    {
        // 移除监听器，避免重复调用
        hammerOBJ.GetComponent<SkeletonAnimation>().AnimationState.Complete -= AnimationState_Complete;

        // 如果锤子正在返回，不执行点击效果
        if (isHammerReturning) return;

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
            isPlaying = false;
            return;
        }
        else
        {
            recordCount += 1;
            jerryOBJ.transform.SetParent(GetJerryPos(), false);
        }
        hammerOBJ.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "Stay", false);
        Debug.Log($"点击了老鼠，当前次数: {recordCount}");
    }

}
