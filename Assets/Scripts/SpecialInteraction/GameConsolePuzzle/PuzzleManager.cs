using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private GameObject cat_087;

    public List<PuzzlePiece> puzzlePieces;
    public List<Transform> targetPositions;


    private float moveStep = 50;
    private float rotateStep = 90f;
    private float snapDistance = 0.5f;
    private float rotationThreshold = 15f;

    private int currentPieceIndex = 0;
    private PuzzlePiece activePiece;


    private GameObject puzzleOBJ;           // 拼图对象引用

    // UI 按钮引用
    public Button upButton, downButton, leftButton, rightButton;
    public Button rotateCWButton;

    public GameObject tipOBJ;       // 通关提醒

    private void Awake()
    {
        // 按钮事件绑定
        upButton.onClick.AddListener(MoveUp);
        downButton.onClick.AddListener(MoveDown);
        leftButton.onClick.AddListener(MoveLeft);
        rightButton.onClick.AddListener(MoveRight);
        rotateCWButton.onClick.AddListener(RotateClockwise);
    }

    void Update()
    {
        // 实时检测当前拼图块是否在正确位置
        if(activePiece != null && !activePiece.isPlaced)
        {
            CheckPiecePosition();
        }
    }

    void MoveUp() => activePiece?.Move(Vector2.up * moveStep);
    void MoveDown() => activePiece?.Move(Vector2.down * moveStep);
    void MoveLeft() => activePiece?.Move(Vector2.left * moveStep);
    void MoveRight() => activePiece?.Move(Vector2.right * moveStep);
    void RotateClockwise() => activePiece?.Rotate(-rotateStep);


    //打开积木拼图界面
    public void OpenPuzzle()
    {
        inputManager.SetUIOpenState(true);
        this.transform.GetChild(0).gameObject.SetActive(true);

        // 初始化第一个拼图块
        if (puzzlePieces.Count > 0)
        {
            activePiece = puzzlePieces[0];
            activePiece.SetActive(true);
        }
    }

    //拼图完成
    public void ClosePuzzle()
    {
        inputManager.SetUIOpenState(false);
        this.transform.GetChild(0).gameObject.SetActive(true);

        cat_087.GetComponent<SpriteRenderer>().enabled = true;
        cat_087.GetComponent<Collider2D>().enabled = true;

        //播放特效、关闭界面

    }


    // 显示提醒框
    public void OpenTip()
    {
        tipOBJ.SetActive(true);
    }

    // 检查当前拼图块位置
    void CheckPiecePosition()
    {
        Transform target = targetPositions[currentPieceIndex];
        float distance = Vector2.Distance(activePiece.transform.position, target.position);
        float angleDiff = Quaternion.Angle(activePiece.transform.rotation, target.rotation);

        if (distance < snapDistance && angleDiff < rotationThreshold)
        {
            // 位置正确，锁定当前拼图
            activePiece.transform.position = target.position;
            activePiece.transform.rotation = target.rotation;
            activePiece.isPlaced = true;
            activePiece.SetActive(false);

            // 激活下一块拼图
            currentPieceIndex++;

            if (currentPieceIndex < puzzlePieces.Count)
            {
                activePiece = puzzlePieces[currentPieceIndex];
                activePiece.SetActive(true);
            }
            else
            {
                // 触发拼图完成事件
                activePiece = null;
                Debug.Log("拼图完成！");
                ClosePuzzle();
            }
        }
    }
}
