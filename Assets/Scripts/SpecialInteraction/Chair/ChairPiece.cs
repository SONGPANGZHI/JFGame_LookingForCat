using System.Collections;
using UnityEngine;

// 拼图碎片
public class ChairPiece : MonoBehaviour
{
    public Transform targetPosition; // 对应的目标位置
    public float moveSpeed = 5f;     // 移动速度
    public float rotationSpeed = 5f; // 旋转速度

    private bool isMoving = false;
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        // 记录初始位置和旋转
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void OnPointerClick()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveToTarget());
        }
    }

    IEnumerator MoveToTarget()
    {
        isMoving = true;

        float progress = 0f;

        while (progress < 1f)
        {
            progress += Time.deltaTime * moveSpeed;

            // 移动位置
            transform.position = Vector3.Lerp(startPosition, targetPosition.position, progress);

            // 旋转到目标方向
            transform.rotation = Quaternion.Lerp(startRotation, targetPosition.rotation, progress);

            yield return null;
        }

        // 确保最终位置和旋转完全匹配
        transform.position = targetPosition.position;
        transform.rotation = targetPosition.rotation;

        isMoving = false;
    }
}
