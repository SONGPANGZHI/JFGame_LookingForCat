using Spine.Unity;
using System.Collections;
using UnityEngine;

/// <summary>
///  001 火把猫
/// </summary>
public class TorchCat : MonoBehaviour
{
    public SkeletonAnimation tailAnim;
    public SkeletonAnimation catAnim;
    public GameObject woodOBJ;
    public SpriteRenderer fireOBJ;
    public Sprite fireSprite;

    public Transform targetPosition;
    private Vector3 startPosition;

    private float moveSpeed = 5f; // 移动速度
    private bool isMoving = false;

    private void Start()
    {
        startPosition = woodOBJ.transform.position;

        bool isCompleted = GameManager.Instance.progressManager.IsCatFound(3);
        if (isCompleted)
        {
            CloseTail();
            catAnim.gameObject.SetActive(true);
            woodOBJ.SetActive(false);
            catAnim.GetComponent<InteractiveCat>().PlayAnim(0, "Sports", false);
        }
        else
        {
            catAnim.gameObject.SetActive(false);
        }

    }

    public void ClickWood()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveToTarget());
        }

        CloseTail();
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
            woodOBJ.transform.position = Vector3.Lerp(startPosition, targetPosition.position, progress);
            yield return null;
        }

        // 确保最终位置和旋转完全匹配
        woodOBJ.transform.position = targetPosition.position;
        woodOBJ.SetActive(false);
        isMoving = false;
    }
    
    /// <summary>
    /// 关闭猫尾巴
    /// </summary>
    public void CloseTail()
    {
        catAnim.gameObject.SetActive(true);
        tailAnim.gameObject.SetActive(false);
        fireOBJ.sprite = fireSprite;
        catAnim.GetComponent<InteractiveCat>().OnObjectInteracted();
    }
}
