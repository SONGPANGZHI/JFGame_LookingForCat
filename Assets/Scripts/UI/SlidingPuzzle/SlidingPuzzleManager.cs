using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPuzzleManager : MonoBehaviour
{
    [Header("拼图设置")]
    public int gridSize = 3; // 3x3 网格
    public float tileSpacing = 5f;
    public Sprite[] puzzleSprites; // 8张图片 + 1张空白图

    [Header("UI引用")]
    public Transform puzzleGrid;
    public GameObject tilePrefab;

    private SlidingPuzzleTile[,] tiles;
    private Vector2Int emptyTilePos;
    private bool isMoving = false;

    // 存储每个位置应该对应的正确图片索引
    private int[,] correctTileIndices;

    private void Awake()
    {
        InitializeCorrectIndices();
        InitializePuzzle();
    }

    void Start()
    {
        ShufflePuzzle();
    }


    // 打开拼图界面
    public void OpenPuzzle()
    { 
        
    }


    void InitializeCorrectIndices()
    {
        correctTileIndices = new int[gridSize, gridSize];
        int index = 0;

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                if (x == gridSize - 1 && y == gridSize - 1)
                {
                    // 最后一个位置是空的，没有图片索引
                    correctTileIndices[x, y] = -1;
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
                // 最后一个位置是空的
                if (x == gridSize - 1 && y == gridSize - 1)
                {
                    emptyTilePos = new Vector2Int(x, y);
                    continue;
                }

                GameObject tileObj = Instantiate(tilePrefab, puzzleGrid);
                SlidingPuzzleTile tile = tileObj.GetComponent<SlidingPuzzleTile>();

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
        // 简单打乱算法
        for (int i = 0; i < 100; i++)
        {
            List<Vector2Int> possibleMoves = GetPossibleMoves();
            if (possibleMoves.Count > 0)
            {
                Vector2Int randomMove = possibleMoves[Random.Range(0, possibleMoves.Count)];
                MoveTile(randomMove);
            }
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
        // 检查所有拼图块是否在正确的位置并且显示正确的图片
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                // 跳过空格位置
                if (x == emptyTilePos.x && y == emptyTilePos.y)
                {
                    // 检查空格是否在正确的位置（右下角）
                    if (x != gridSize - 1 || y != gridSize - 1)
                    {
                        return; // 空格不在正确位置
                    }
                    continue;
                }

                SlidingPuzzleTile tile = tiles[x, y];
                if (tile == null)
                {
                    return; // 不应该有空的位置（除了空格）
                }

                // 检查拼图块是否在正确的位置并且有正确的图片索引
                if (tile.currentPos.x != x || tile.currentPos.y != y)
                {
                    return; // 位置不正确
                }

                // 检查这个位置上的拼图块是否有正确的图片索引
                int correctIndex = correctTileIndices[x, y];
                if (tile.spriteIndex != correctIndex)
                {
                    return; // 图片不正确
                }
            }
        }

        Debug.Log("恭喜！拼图完成！");
    }
}
