using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    public List<PuzzlePiece> puzzlePieces;
    public List<Transform> targetPositions;
    public float moveStep = 0.5f;
    public float rotateStep = 15f;
    public float snapDistance = 0.5f;
    public float rotationThreshold = 15f;

    private int currentPieceIndex = 0;
    private PuzzlePiece activePiece;

    public Button upButton, downButton, leftButton, rightButton;
    public Button rotateCWButton;
    public Button confirmButton;

    void Start()
    {
        // 初始化第一个拼图块
        if (puzzlePieces.Count > 0)
        {
            activePiece = puzzlePieces[0];
            activePiece.SetActive(true);
        }

        // 按钮事件绑定
        upButton.onClick.AddListener(MoveUp);
        downButton.onClick.AddListener(MoveDown);
        leftButton.onClick.AddListener(MoveLeft);
        rightButton.onClick.AddListener(MoveRight);
        rotateCWButton.onClick.AddListener(RotateClockwise);
        //rotateCCWButton.onClick.AddListener(RotateCounterClockwise);
    }

    void Update()
    {
        // 实时检测当前拼图块是否在正确位置
        if (activePiece != null && !activePiece.isPlaced)
        {
            CheckPiecePosition();
        }
    }

    void MoveUp() => activePiece?.Move(Vector2.up * moveStep);
    void MoveDown() => activePiece?.Move(Vector2.down * moveStep);
    void MoveLeft() => activePiece?.Move(Vector2.left * moveStep);
    void MoveRight() => activePiece?.Move(Vector2.right * moveStep);
    void RotateClockwise() => activePiece?.Rotate(-rotateStep);

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
                activePiece = null;
                Debug.Log("拼图完成！");
                // 这里可以触发拼图完成事件
            }
        }
    }
}
