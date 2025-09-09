using System.Collections;
using UnityEngine;

// 组装拼图碎片
public class AssemblePuzzlePiece : MonoBehaviour
{
    public int Cat_ID;
    public Transform targetPosition; // 对应的目标位置


    private float moveSpeed = 5f;     // 移动速度
    private float rotationSpeed = 5f;
    private bool isMoving = false;
    private Vector3 startPosition;
    private Vector3 startScale;
    private Quaternion startRotation;
    private bool compele = false;

    void Start()
    {
        // 记录初始位置和旋转
        startPosition = transform.position;
        startRotation = transform.rotation;
        startScale = transform.localScale;
    }

    // 拼图碎片已完成
    public void OnPuzzlePieceCompleted()
    {
        compele = true;
        transform.position = targetPosition.position;
        transform.rotation = targetPosition.rotation;
    }

    // 当点击
    public void OnPointerClick()
    {
        if (!isMoving && !compele)
        {
            StartCoroutine(MoveToTarget());
        }
    }

    // 碎片移动到目标位置
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
            transform.localScale = Vector3.Lerp(startScale, Vector3.one, progress);

            yield return null;
        }

        // 确保最终位置和旋转完全匹配
        transform.position = targetPosition.position;
        transform.rotation = targetPosition.rotation;
        transform.localScale = Vector3.one;
        AssemblePuzzleManager.Instance.PieceAssembled(Cat_ID);
        compele = true;
        isMoving = false;
    }
}
