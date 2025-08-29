using Spine.Unity;
using UnityEngine;

/// <summary>
///  003 火把猫 001
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

    public UniversalMovementController universalMovement;

    public SkeletonAnimation fishAnim;
    public GameObject obstacle;

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

        if (GameManager.Instance.progressManager.IsCatFound(1))
        {
            obstacle.SetActive(false);
            fishAnim.GetComponent<MeshRenderer>().enabled = true;
            fishAnim.state.SetAnimation(0, "Sports", false);
        }

    }
     
    /// <summary>
    /// 点击树
    /// </summary>
    public void ClickWood()
    {
        if (!isMoving)
        {

            universalMovement.StartMoveWithSpeed(woodOBJ.transform, targetPosition,3, () => 
            {
                woodOBJ.SetActive(false);
            });

            CloseTail();
        }
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

    /// <summary>
    /// 点击鱼
    /// </summary>
    public void FishClick()
    {
        obstacle.transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
        obstacle.SetActive(false);
        fishAnim.GetComponent<MeshRenderer>().enabled = true;
        fishAnim.state.SetAnimation(0, "Cat_up", false);
        Invoke("OpenColider",1f);
    }

    public void OpenColider()
    {
        fishAnim.GetComponent<Collider2D>().enabled = true;
    }
}
