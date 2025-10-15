using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AppleRoll : MonoBehaviour
{
    public Transform targetPoint;    // 目标点Transform
    public InteractiveCat interactiveCat;
    private float speed = 3f;         // 移动和旋转速度

    private bool _clickApple = false;
    private bool arrivePoint = true;

    private void Start()
    {
        bool isCompleted = GameManager.Instance.progressManager.IsCatFound(8);
        if (isCompleted)
        {
            arrivePoint = false;
            transform.position = targetPoint.position;
        }
    }

    /// <summary>
    /// 点击苹果
    /// </summary>
    public void ClickApple()
    {
        _clickApple = true;
    }


    void Update()
    {
        if (_clickApple && arrivePoint)
        {
            // 检查目标点是否存在
            if (targetPoint == null)
                return;

            // 计算到目标的距离
            float distance = Vector3.Distance(transform.position, targetPoint.position);

            // 如果还没到达目标
            if (distance > 0.1f)
            {
                // 朝目标移动
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPoint.position,
                    speed * Time.deltaTime
                );

                // 同时旋转（绕Z轴滚动）
                transform.Rotate(0, 0, speed * 360 * Time.deltaTime);
            }
            else
            {
                arrivePoint = false;
                // 精确停在目标点
                transform.position = targetPoint.position;
                interactiveCat.OnObjectInteracted();
            }
        }

        
    }
}
