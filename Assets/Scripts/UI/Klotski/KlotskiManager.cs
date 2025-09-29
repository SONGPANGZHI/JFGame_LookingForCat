using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KlotskiManager : MonoBehaviour
{
    public static KlotskiManager Instance;

    public UniversalMovementController playerController;

    [Header("Grid Settings")]
    public int gridSize = 6;
    public float cellSize = 120f;
    public Transform gridParent;
    public GameObject cellPrefab;

    [Header("Game Objects")]
    public GameObject horizontalBlockPrefab;
    public GameObject verticalBlockPrefab;
    public GameObject verticalBlockPrefab_2;
    public GameObject targetBlockPrefab;
    public GameObject exitPrefab;

    private KlotskiBlock[,] grid;
    private KlotskiBlock targetBlock;
    private KlotskiBlock exitBlock;
    private List<KlotskiBlock> movableBlocks = new List<KlotskiBlock>();
    private Vector2Int exitPosition = new Vector2Int(5, 2); 

    public bool isGameOver = false;

    [Header("Cat")]
    public GameObject klotskiPlane;
    public SkeletonAnimation catAnim_087;
    public GameObject catAnimPlay;

    //public GameObject ClickPlay;
    //public Transform catTrans;
    //public Transform tragetPos;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializeGrid();
        SetupGame();
        JudagCatUnlock();
    }


    void InitializeGrid()
    {
        grid = new KlotskiBlock[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                GameObject cell = Instantiate(cellPrefab, gridParent);
                cell.transform.localPosition = GetWorldPosition(x, y);
            }
        }
    }

    void SetupGame()
    {
      
        CreateExit(exitPosition.x, exitPosition.y);

        targetBlock = CreateBlock(BlockType.Target, 0, 2, 2, 1); 

        CreateBlock(BlockType.Vertical, 3, 1, 3, 1); 
        CreateBlock(BlockType.Vertical, 2, 1, 3, 2); 
        CreateBlock(BlockType.Vertical, 5, 1, 3, 3);

        CreateBlock(BlockType.Horizontal, 0, 3, 2, 4); 
        CreateBlock(BlockType.Vertical_2, 0, 4, 2, 5); 
        CreateBlock(BlockType.Horizontal, 1, 5, 2, 6);
    }

    KlotskiBlock CreateBlock(BlockType type, int startX, int startY, int length, int instanceID)
    {
        GameObject blockObj = null;
        Vector2Int[] positions = new Vector2Int[length];

        switch (type)
        {
            case BlockType.Horizontal:
                blockObj = Instantiate(horizontalBlockPrefab, gridParent);
                for (int i = 0; i < length; i++)
                {
                    positions[i] = new Vector2Int(startX + i, startY);
                }
                break;

            case BlockType.Vertical:
                blockObj = Instantiate(verticalBlockPrefab, gridParent);
                for (int i = 0; i < length; i++)
                {
                    positions[i] = new Vector2Int(startX, startY + i);
                }
                break;

            case BlockType.Vertical_2:
                blockObj = Instantiate(verticalBlockPrefab_2, gridParent);
                for (int i = 0; i < length; i++)
                {
                    positions[i] = new Vector2Int(startX, startY + i);
                }
                break;
            case BlockType.Target:
                blockObj = Instantiate(targetBlockPrefab, gridParent);
                for (int i = 0; i < length; i++)
                {
                    positions[i] = new Vector2Int(startX + i, startY);
                }
                break;
        }

        KlotskiBlock block = blockObj.GetComponent<KlotskiBlock>();
        block.Initialize(type, positions, instanceID);

        foreach (Vector2Int pos in positions)
        {
            if (IsValidPosition(pos.x, pos.y))
            {
                grid[pos.x, pos.y] = block;
            }
        }

        if (type != BlockType.Target)
        {
            movableBlocks.Add(block);
        }

        
        Vector2 centerPos = GetCenterPosition(positions);
        blockObj.transform.localPosition = GetWorldPosition(centerPos.x, centerPos.y);

        return block;
    }

    void CreateExit(int x, int y)
    {
        GameObject exitObj = Instantiate(exitPrefab, gridParent);
        exitObj.transform.localPosition = GetWorldPosition(x, y);
        exitBlock = exitObj.GetComponent<KlotskiBlock>();
    }

    
    Vector3 GetWorldPosition(float x, float y)
    {
        return new Vector3(x * cellSize, -y * cellSize, 0);
    }

    Vector2 GetCenterPosition(Vector2Int[] positions)
    {
        Vector2 center = Vector2.zero;
        foreach (Vector2Int pos in positions)
        {
            center += new Vector2(pos.x, pos.y);
        }
        return center / positions.Length;
    }

    public bool CanMoveBlock(KlotskiBlock block, Vector2Int direction)
    {
        
        foreach (Vector2Int pos in block.positions)
        {
            Vector2Int newPos = pos + direction;

            if (!IsValidPosition(newPos.x, newPos.y))
                return false;

            if (grid[newPos.x, newPos.y] != null && grid[newPos.x, newPos.y] != block)
                return false;
        }

        return true;
    }

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="block"></param>
    /// <param name="direction"></param>
    public void MoveBlock(KlotskiBlock block, Vector2Int direction)
    {
        if (isGameOver) return;

        foreach (Vector2Int pos in block.positions)
        {
            grid[pos.x, pos.y] = null;
        }

       
        Vector2Int[] newPositions = new Vector2Int[block.positions.Length];
        for (int i = 0; i < block.positions.Length; i++)
        {
            newPositions[i] = block.positions[i] + direction;
        }
        block.positions = newPositions;

       
        foreach (Vector2Int pos in newPositions)
        {
            grid[pos.x, pos.y] = block;
        }

        
        Vector2 centerPos = GetCenterPosition(newPositions);
        block.transform.localPosition = GetWorldPosition(centerPos.x, centerPos.y);

        
        CheckGameOver();
    }


    /// <summary>
    /// 检查游戏是否结束
    /// </summary>
    void CheckGameOver()
    {
       
        foreach (Vector2Int pos in targetBlock.positions)
        {
            if (pos == exitPosition)
            {
                GameOver(true);
                return;
            }
        }
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    /// <param name="success"></param>
    void GameOver(bool success)
    {
        isGameOver = true;
        

        Invoke("CatMove",1f);
        
    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public void RestartGame()
    {
        foreach (KlotskiBlock block in movableBlocks)
        {
            if (block != null) Destroy(block.gameObject);
        }
        if (targetBlock != null) Destroy(targetBlock.gameObject);
        if (exitBlock != null) Destroy(exitBlock.gameObject);

        movableBlocks.Clear();

       
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                grid[x, y] = null;
            }
        }

        isGameOver = false;
        SetupGame();
    }

    bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < gridSize && y >= 0 && y < gridSize;
    }


    /// <summary>
    /// 打开华容道界面
    /// </summary>
    public void OpenKlotskiPlane()
    {
        klotskiPlane.SetActive(true);
        UIManager.Instance.OtherParameters(false);
    }

    /// <summary>
    /// 关闭
    /// </summary>
    public void CloseKlotskiPlane()
    {
        klotskiPlane.SetActive(false);
        RestartGame();
        UIManager.Instance.OtherParameters(true);
    }

    public void CatMove()
    {
        klotskiPlane.SetActive(false);
        UIManager.Instance.OtherParameters(true);
        catAnimPlay.SetActive(false);
        catAnim_087.GetComponent<MeshRenderer>().enabled = true;
        catAnim_087.GetComponent<Collider2D>().enabled = true;
        catAnim_087.enabled = true;
        catAnim_087.state.SetAnimation(0, "Sports", true);

    }

    public void JudagCatUnlock()
    {
        bool isCompleted = GameManager.Instance.progressManager.IsCatFound(87);
        if (isCompleted)
        {
            catAnimPlay.SetActive(false);
            catAnim_087.GetComponent<MeshRenderer>().enabled = true;
            catAnim_087.GetComponent<Collider2D>().enabled = true;
            catAnim_087.enabled = true;
            catAnim_087.state.SetAnimation(0, "Sports", true);
        }
    }
}
