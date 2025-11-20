using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridManager: MonoBehaviour
{
    public static GridManager Instance;

    [Header("Grid Settings")]
    public int gridSize = 7;
    public GameObject cellPrefab;
    public GridCell[,] gridCells;

    [Header("Game Settings")]
    public int pointCount = 6;
    public ColorType[] pointColorTypes = {
        ColorType.Red, ColorType.Green, ColorType.Blue
    };

    [Header("UI References")]
    public Transform gridContainer;
    public Button switchPoint;
    public List<Sprite> iconSprite;

    [Header("Mobile Settings")]
    public float touchSensitivity = 15f;

    [Header("Cat Settings")]
    public GameObject cat_Id_078;
    public GameObject cat_Id_077;
    public SpriteRenderer SpriteRenderer;
    public Sprite newSprite;



    private bool isDrawing = false;
    private GridCell startCell;
    private GridCell currentCell;
    private ColorType currentPathColorType;
    private Color currentPathColor;
    private List<GridCell> currentPath = new List<GridCell>();
    private List<GridCell> tempPath = new List<GridCell>();
    private List<LinePath> completedPaths = new List<LinePath>();
    private int completedPairs = 0;
    private Vector2 lastTouchPosition;

    private bool startPaly = false;

    public bool IsDrawing => isDrawing;

    [System.Serializable]
    public class LinePath
    {
        public List<GridCell> pathCells = new List<GridCell>();
        public ColorType pathColorType;
        public Color pathColor;
        public bool isCompleted;
    }


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        switchPoint.onClick.AddListener(ResetGame);
        LoadGridMap();
    }

    private void Start()
    {
        bool isCompleted = GameManager.Instance.progressManager.IsCatFound(77);
        bool Completed = GameManager.Instance.progressManager.IsCatFound(78);

        if (isCompleted || Completed)
        {
            ShowCat();
        }

    }

    /// <summary>
    /// 胜利显示猫猫
    /// </summary>
    public void ShowCat()
    {
        startPaly = false;
        cat_Id_078.GetComponent<SpriteRenderer>().enabled = true;
        cat_Id_078.GetComponent<Collider2D>().enabled = true;
        cat_Id_077.GetComponent<SpriteRenderer>().enabled = true;
        cat_Id_077.GetComponent<Collider2D>().enabled = true;
        SpriteRenderer.sprite = newSprite;
        UIManager.Instance.OtherParameters(true);
        this.gameObject.SetActive(false);

    }

    private bool tennisGameBool = false;

    /// <summary>
    /// 开始网球游戏
    /// </summary>
    public void StartPlay()
    {
        if(!tennisGameBool)
        {
            startPaly = true;
            UIManager.Instance.OtherParameters(false);
            transform.GetChild(0).gameObject.SetActive(true);
            if (gridContainer.childCount > 0) return;
            else
            {
                InitializeGrid();
                PlacePoints();
            }
        }
    }

    /// <summary>
    /// 打开提醒框
    /// </summary>
    public void OpenTips()
    {
        Invoke("ShowCat",1f);
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    public void ClosePlane()
    {
        UIManager.Instance.OtherParameters(true);
        transform.GetChild(0).gameObject.SetActive(false);
        ResetGame();
    }

    #region 读取格子表

    private List<SpointMapConfig> allMaps = new List<SpointMapConfig>();

    /// <summary>
    /// 加载格子地图
    /// </summary>
    public void LoadGridMap()
    {
        string gridMapLoad = Resources.Load<TextAsset>("SpointMap").text;
        string wrappedJson = "{\"items\":" + gridMapLoad + "}";
        SpointMapArrayWrapper wrapper = JsonUtility.FromJson<SpointMapArrayWrapper>(wrappedJson);
        for (int i = 0; i < wrapper.items.Length; i++)
        {
            allMaps.Add(wrapper.items[i]);
        }
    }

    #endregion

    void InitializeGrid()
    {
        gridCells = new GridCell[gridSize, gridSize];

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                GameObject cellObj = Instantiate(cellPrefab, gridContainer);
                GridCell cell = cellObj.GetComponent<GridCell>();
                cell.Initialize(x, y);
                // 默认设置为红色（空白）
                cell.SetColorType(ColorType.White);
                gridCells[x, y] = cell;
            }
        }
    }

    void PlacePoints()
    {
        List<Vector2> occupiedPositions = new List<Vector2>();
        int randomID = Random.Range(0, allMaps.Count);

        foreach (var cell in allMaps[randomID].specialCells)
        {
            Vector2 pos = new Vector2(cell.x, cell.y);
            occupiedPositions.Add(pos);

            Sprite _SpriteType = StringToSpriteType(cell.SpritType);
            gridCells[cell.x, cell.y].InitSprite(_SpriteType, cell.SpritType);
            
        }
    }

    private Sprite StringToSpriteType(string colorString)
    {
        switch (colorString)
        {
            case "CowCat": return iconSprite[0];
            case "Cat": return iconSprite[1];
            case "Ball": return iconSprite[2];
        }

        return null;
    }

    private ColorType StringToColorType(string colorString)
    {
        switch (colorString.ToLower())
        {
            case "red": return ColorType.Red;
            case "green": return ColorType.Green;
            case "blue": return ColorType.Blue;
            default: return ColorType.White;
        }
    }


    void Update()
    {
        if(startPaly)
            HandleMobileInput();
    }

    void HandleMobileInput()
    {
        if (!startPaly)  return; 

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    lastTouchPosition = touch.position;
                    HandleTouchBegan(touch.position);
                    break;

                case TouchPhase.Moved:
                    if (isDrawing)
                    {
                        HandleTouchMovement(touch.position);
                    }
                    break;

                case TouchPhase.Ended:
                    if (isDrawing)
                    {
                        HandleTouchEnded(touch.position);
                    }
                    break;
            }
        }
    }

    void HandleTouchBegan(Vector2 touchPosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = touchPosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            GridCell cell = result.gameObject.GetComponent<GridCell>();
            if (cell != null)
            {
                HandleCellClick(cell);
                break;
            }
        }
    }

    void HandleTouchMovement(Vector2 touchPosition)
    {
        if (Vector2.Distance(touchPosition, lastTouchPosition) > touchSensitivity)
        {
            lastTouchPosition = touchPosition;

            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = touchPosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            foreach (RaycastResult result in results)
            {
                GridCell cell = result.gameObject.GetComponent<GridCell>();
                if (cell != null)
                {
                    HandleCellHover(cell);
                    break;
                }
            }
        }
    }

    public void HandleCellClick(GridCell cell)
    {
        if (isDrawing)
        {
            return;
        }

        // 开始新路径 - 只能点击彩色起点
        if (cell.isOccupied && !cell.isPath && IsOriginalPoint(cell) && !IsPointConnected(cell))
        {
            StartNewPath(cell);
        }
    }

    public void HandleCellHover(GridCell cell)
    {
        if (!isDrawing || cell == null) return;

        if (IsCompletedPathCell(cell) && cell != startCell)
        {
            ResetCurrentPath();
            return;
        }

        if (CanMoveToCell(cell))
        {
            ExtendPath(cell);
        }
        else if (cell != currentCell)
        {
            TryBacktrackPath(cell);
        }
    }

    bool CanMoveToCell(GridCell cell)
    {
        if (cell == null) return false;
        if (cell == currentCell) return true;

        int dx = Mathf.Abs(cell.x - currentCell.x);
        int dy = Mathf.Abs(cell.y - currentCell.y);
        bool isAdjacent = (dx == 1 && dy == 0) || (dx == 0 && dy == 1);

        if (!isAdjacent) return false;

        if (cell.isOccupied)
        {
            return cell.colorType == currentPathColorType && IsOriginalPoint(cell) && !IsPointConnected(cell);
        }
        else
        {
            return !cell.isPath || (cell.isPath && !IsCompletedPathCell(cell));
        }
    }

    void StartNewPath(GridCell startCell)
    {
        this.startCell = startCell;
        this.currentCell = startCell;
        this.currentPathColorType = startCell.colorType;
        this.currentPathColor = GetColorFromType(currentPathColorType);
        this.isDrawing = true;
        this.currentPath = new List<GridCell> { startCell };

        startCell.SetAsPath(currentPathColorType, true, false);
    }

    void ExtendPath(GridCell nextCell)
    {
        if (IsValidEndPoint(nextCell))
        {
            CompletePath(nextCell);
            return;
        }

        if (currentPath.Contains(nextCell) && nextCell != currentCell)
        {
            int index = currentPath.IndexOf(nextCell);
            BacktrackToIndex(index);
            return;
        }

        if (nextCell == startCell && currentPath.Count > 1)
        {
            CancelCurrentPath();
            return;
        }

        currentPath.Add(nextCell);
        nextCell.SetAsPath(currentPathColorType);
        currentCell = nextCell;

        RestoreTempPathColors();
    }

    void CompletePath(GridCell endCell)
    {
        if (!IsValidEndPoint(endCell) || currentPath.Count <= 1)
        {
            ResetCurrentPath();
            return;
        }

        tempPath.Clear();
        endCell.SetAsPath(currentPathColorType, false, true);

        LinePath completedPath = new LinePath
        {
            pathCells = new List<GridCell>(currentPath),
            pathColorType = currentPathColorType,
            pathColor = currentPathColor,
            isCompleted = true
        };
        completedPaths.Add(completedPath);

        completedPairs++;
        isDrawing = false;


        if (completedPairs >= pointColorTypes.Length)
        {
            OpenTips();
            tennisGameBool = true;
            Debug.Log("游戏完成！");
        }

        currentPath.Clear();
    }

    // 辅助方法：从ColorType获取颜色
    private Color GetColorFromType(ColorType type)
    {
        switch (type)
        {
            case ColorType.Red: return Color.red;
            case ColorType.Green: return Color.green;
            case ColorType.Blue: return Color.blue;
            case ColorType.White: return Color.white;
            default: return Color.white;
        }
    }

    public void ResetGame()
    {
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                gridCells[x, y].ResetCell();
            }
        }

        completedPaths.Clear();
        PlacePoints();
        isDrawing = false;
        completedPairs = 0;
        currentPath.Clear();
        tempPath.Clear();

    }

    void ResetCurrentPath()
    {
        // 重置所有当前路径的格子（除了起点）
        foreach (GridCell cell in currentPath)
        {
            if (cell != startCell && !IsCompletedPathCell(cell))
            {
                cell.ResetCell();
            }
        }

        // 清除临时路径
        tempPath.Clear();

        isDrawing = false;
        currentPath.Clear();
    }

    void RestoreTempPathColors()
    {
        // 恢复临时路径中在当前路径上的格子颜色
        List<GridCell> cellsToRemove = new List<GridCell>();

        foreach (GridCell cell in tempPath)
        {
            if (currentPath.Contains(cell))
            {
                cell.SetAsPath(currentPathColorType);
                cellsToRemove.Add(cell);
            }
        }

        // 移除已恢复的格子
        foreach (GridCell cell in cellsToRemove)
        {
            tempPath.Remove(cell);
        }
    }

    public void CancelCurrentPath()
    {
        if (!isDrawing) return;

        // 重置所有当前路径的格子（除了起点）
        foreach (GridCell cell in currentPath)
        {
            if (cell != startCell && !IsCompletedPathCell(cell))
            {
                cell.ResetCell();
            }
            else if (cell == startCell)
            {
                cell.ResetCell(true); // 保留起点颜色
            }
        }

        // 清除临时路径
        tempPath.Clear();

        isDrawing = false;
        currentPath.Clear();
    }

    // 新增：处理手指抬起时的路径优化
    void HandleTouchEnded(Vector2 touchPosition)
    {
        if (isDrawing)
        {
            // 检查最后触摸的格子
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = touchPosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            GridCell endCell = null;
            foreach (RaycastResult result in results)
            {
                endCell = result.gameObject.GetComponent<GridCell>();
                if (endCell != null) break;
            }

            // 如果是有效的终点，完成路径
            if (endCell != null && IsValidEndPoint(endCell))
            {
                CompletePath(endCell);
            }
            else
            {
                // 如果不是有效终点，检查是否需要回溯
                if (endCell != null && currentPath.Contains(endCell) && endCell != currentCell)
                {
                    int index = currentPath.IndexOf(endCell);
                    BacktrackToIndex(index);
                }
                else
                {
                    // 完全重置路径
                    ResetCurrentPath();
                }
            }
        }
    }

    private bool IsOriginalPoint(GridCell cell)
    {
        // 原始的点是有颜色但不是路径状态，或者是起点/终点
        return (cell.isOccupied && !cell.isPath && cell.cellColor != Color.white) ||
               cell.isStartPoint || cell.isEndPoint;
    }

    bool IsPointConnected(GridCell cell)
    {
        foreach (LinePath path in completedPaths)
        {
            if (path.pathCells.Contains(cell) && path.isCompleted)
            {
                return true;
            }
        }
        return false;
    }

    bool IsCompletedPathCell(GridCell cell)
    {
        foreach (LinePath path in completedPaths)
        {
            if (path.pathCells.Contains(cell) && path.isCompleted && cell != path.pathCells[0] && cell != path.pathCells[path.pathCells.Count - 1])
            {
                return true;
            }
        }
        return false;
    }

    bool IsValidEndPoint(GridCell cell)
    {
        return cell != startCell &&
               cell.isOccupied &&
               cell.cellColor == currentPathColor &&
               IsOriginalPoint(cell) &&
               !IsPointConnected(cell);
    }

    void BacktrackToIndex(int targetIndex)
    {
        // 取消从targetIndex+1开始的所有格子
        for (int i = currentPath.Count - 1; i > targetIndex; i--)
        {
            GridCell cellToReset = currentPath[i];
            if (!IsCompletedPathCell(cellToReset) && cellToReset != startCell)
            {
                cellToReset.ResetCell();
                tempPath.Add(cellToReset); // 添加到临时路径，用于颜色恢复
            }
            currentPath.RemoveAt(i);
        }

        currentCell = currentPath[targetIndex];
    }

    void TryBacktrackPath(GridCell targetCell)
    {
        // 检查目标格子是否在当前路径中（回溯）
        int targetIndex = currentPath.IndexOf(targetCell);
        if (targetIndex > 0 && targetIndex < currentPath.Count - 1)
        {
            // 回溯到该格子，取消之后的路径
            BacktrackToIndex(targetIndex);
        }
    }


    
}
