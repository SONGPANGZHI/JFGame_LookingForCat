using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SlidingPuzzleManager : MonoBehaviour
{
    [Header("拼图设置")]
    public int gridSize = 3; // 3x3 网格
    public float tileSpacing = 5f;
    public Sprite[] puzzleSprites; // 8张图片 + 1张空白图

    [Header("UI引用")]
    public Transform puzzleGrid;
    public GameObject tilePrefab;

    public GameObject puzzlePlane;
    public SpriteRenderer catID_122;
    


    private SlidingPuzzleTile[,] tiles;
    private Vector2Int emptyTilePos;
    private bool isMoving = false;

    // 存储每个位置应该对应的正确图片索引
    private int[,] correctTileIndices;

    private void Awake()
    {
        InitializeCorrectIndices();
        InitializePuzzle();
        ShufflePuzzle();
    }

    private void Start()
    {
        bool isCompleted = GameManager.Instance.progressManager.IsCatFound(122);
        if (isCompleted)
            UnlockCat_122();
    }

    // 打开拼图界面
    public void OpenPuzzlePlane()
    {
        bool isCompleted = GameManager.Instance.progressManager.IsCatFound(122);

        if (isCompleted)
            return;
        else
        {
            UIManager.Instance.OtherParameters(false);
            puzzlePlane.SetActive(true);
        }

    }

    /// <summary>
    /// 关闭拼图界面
    /// </summary>
    public void ClosePuzzlePlane()
    {
        // 在这里处理拼图界面关闭时的逻辑
        UIManager.Instance.OtherParameters(true);
        puzzlePlane.SetActive(false);
        RestartGame();
    }

    /// <summary>
    /// 胜利逻辑
    /// </summary>
    public void WinPuzzle()
    {
        UIManager.Instance.OtherParameters(true);
        puzzlePlane.SetActive(false);
        UnlockCat_122();
    }

    public void UnlockCat_122()
    {
        catID_122.enabled = true;
        catID_122.GetComponent<Collider2D>().enabled = true;
    }


    void InitializeCorrectIndices()
    {
        correctTileIndices = new int[gridSize, gridSize];
        int index = 0;

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                // 第八个位置（索引7）是空白，对应坐标(1,2)或(2,1)取决于行列顺序
                // 按照您的需求，第八个位置在3x3网格中应该是(2,2)
                if (x == 2 && y == 2) // 第八个位置（从1开始数）
                {
                    correctTileIndices[x, y] = -1; // 空白位置标记为-1
                }
                else
                {
                    correctTileIndices[x, y] = index;
                    index++;
                }
            }
        }
    }

    void InitializePuzzle()
    {
        tiles = new SlidingPuzzleTile[gridSize, gridSize];

        // 清空现有拼图块
        foreach (Transform child in puzzleGrid)
        {
            Destroy(child.gameObject);
        }

        // 创建拼图块
        int spriteIndex = 0;
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                // 第八个位置是空的（坐标2,2）
                if (x == 2 && y == 2)
                {
                    emptyTilePos = new Vector2Int(x, y);
                    continue;
                }

                GameObject tileObj = Instantiate(tilePrefab, puzzleGrid);
                SlidingPuzzleTile tile = tileObj.GetComponent<SlidingPuzzleTile>();
                tile.Init();

                // 设置图片和初始位置
                if (spriteIndex < puzzleSprites.Length)
                {
                    tile.SetTile(puzzleSprites[spriteIndex], new Vector2Int(x, y), spriteIndex);
                    spriteIndex++;
                }

                tiles[x, y] = tile;

                // 设置位置
                RectTransform rect = tileObj.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(
                    x * (rect.rect.width + tileSpacing),
                    -y * (rect.rect.height + tileSpacing)
                );
            }
        }
    }

    void ShufflePuzzle()
    {
        // 简单打乱算法 - 确保打乱后拼图有解
        int shuffleCount = 0;
        int maxShuffles = 1000;

        while (shuffleCount < maxShuffles)
        {
            List<Vector2Int> possibleMoves = GetPossibleMoves();
            if (possibleMoves.Count > 0)
            {
                Vector2Int randomMove = possibleMoves[Random.Range(0, possibleMoves.Count)];
                MoveTile(randomMove);
                shuffleCount++;
            }

            // 确保不会无限循环
            if (shuffleCount >= 100) break;
        }
    }

    public void OnTileClicked(Vector2Int tilePos)
    {
        if (isMoving) return;

        // 检查是否可以移动
        if (CanMove(tilePos))
        {
            MoveTile(tilePos);
            CheckWinCondition();
        }
    }

    bool CanMove(Vector2Int tilePos)
    {
        // 检查是否与空格相邻
        int dx = Mathf.Abs(tilePos.x - emptyTilePos.x);
        int dy = Mathf.Abs(tilePos.y - emptyTilePos.y);

        return (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
    }

    void MoveTile(Vector2Int tilePos)
    {
        SlidingPuzzleTile tile = tiles[tilePos.x, tilePos.y];

        // 交换位置
        tiles[emptyTilePos.x, emptyTilePos.y] = tile;
        tiles[tilePos.x, tilePos.y] = null;

        // 更新拼图块的位置信息
        tile.UpdatePosition(emptyTilePos);

        // 移动动画
        StartCoroutine(MoveTileAnimation(tile, emptyTilePos));

        // 更新空格位置
        emptyTilePos = tilePos;
    }

    IEnumerator MoveTileAnimation(SlidingPuzzleTile tile, Vector2Int targetPos)
    {
        isMoving = true;

        RectTransform rect = tile.GetComponent<RectTransform>();
        Vector2 targetPosition = new Vector2(
            targetPos.x * (rect.rect.width + tileSpacing),
            -targetPos.y * (rect.rect.height + tileSpacing)
        );

        float duration = 0.2f;
        float elapsed = 0f;
        Vector2 startPosition = rect.anchoredPosition;

        while (elapsed < duration)
        {
            rect.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rect.anchoredPosition = targetPosition;
        isMoving = false;
    }

    List<Vector2Int> GetPossibleMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        // 检查上下左右四个方向
        Vector2Int[] directions = {
            new Vector2Int(0, 1),  // 上
            new Vector2Int(1, 0),  // 右
            new Vector2Int(0, -1), // 下
            new Vector2Int(-1, 0)  // 左
        };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int checkPos = emptyTilePos + dir;
            if (IsValidPosition(checkPos) && tiles[checkPos.x, checkPos.y] != null)
            {
                moves.Add(checkPos);
            }
        }

        return moves;
    }

    bool IsValidPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridSize && pos.y >= 0 && pos.y < gridSize;
    }

    void CheckWinCondition()
    {
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                int correctIndex = correctTileIndices[x, y];

                if (correctIndex == -1)
                {
                    // 这一格应该是空白
                    if (tiles[x, y] != null)
                        return;
                }
                else
                {
                    // 这一格应该有 tile
                    SlidingPuzzleTile tile = tiles[x, y];
                    if (tile == null) return;

                    if (tile.spriteIndex != correctIndex)
                    {
                        return;
                    }
                }
            }
        }

        Debug.Log("恭喜！拼图完成！");
        Invoke("WinPuzzle", 2f);
    }

    // 辅助方法：重新开始游戏
    public void RestartGame()
    {
        // 重置所有图块到初始位置
        InitializePuzzle();
        ShufflePuzzle();
    }

    // 辅助方法：显示当前拼图状态（调试用）
    void DebugPuzzleState()
    {
        string state = "当前拼图状态:\n";
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                if (tiles[x, y] == null)
                    state += "空 ";
                else
                    state += tiles[x, y].spriteIndex + " ";
            }
            state += "\n";
        }
        Debug.Log(state);
    }
}
