using System.Collections.Generic;
using UnityEngine;

public class NineSquareGridPuzzle : MonoBehaviour
{
    public Transform gridParent;
    public VisibleCat catID_112;

    private List<PuzzleTile> tiles = new List<PuzzleTile>();
    private int[] correctOrder = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };     // 正确层级的顺序
    private int selectedTileIndex = -1;                             // 当前选中的格子索引
    private bool isGameComplete = false;                            // 游戏是否完成

    void Start()
    {
        InitializeTiles();
    }

    /// <summary>
    /// 初始化 格子
    /// </summary>
    void InitializeTiles()
    {
        // 获取所有子格子
        tiles.Clear();

        for (int i = 0; i < gridParent.childCount; i++)
        {
            Transform child = gridParent.GetChild(i);
            gridParent.GetChild(i).gameObject.name = "ID" + i.ToString();
            PuzzleTile tile = child.GetComponent<PuzzleTile>();
            if (tile != null)
            {
                tile.Initialize(i, this);
                tiles.Add(tile);
            }
        }
    }

    /// <summary>
    /// 随机格子
    /// </summary>
    void ShufflePuzzle()
    {
        // 随机打乱顺序（通过改变SetSiblingIndex）
        List<int> indices = new List<int>();
        for (int i = 0; i < tiles.Count; i++)
        {
            indices.Add(i);
        }

        // Fisher-Yates 洗牌算法
        for (int i = indices.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = indices[i];
            indices[i] = indices[randomIndex];
            indices[randomIndex] = temp;
        }

        // 应用打乱后的顺序
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[indices[i]].transform.SetSiblingIndex(i);
        }

        selectedTileIndex = -1;
        isGameComplete = false;
    }


    /// <summary>
    /// 交换两个格子层级
    /// </summary>
    /// <param name="tileId"></param>
    public void OnTileClicked(int tileId)
    {
        if (isGameComplete) return;

        if (selectedTileIndex == -1)
        {
            // 第一次选择
            selectedTileIndex = tileId;
            tiles[tileId].SetSelected(true);
        }
        else
        {
            // 第二次选择，交换位置
            tiles[selectedTileIndex].SetSelected(false);

            if (selectedTileIndex != tileId)
            {
                SwapTiles(selectedTileIndex, tileId);
                CheckCompletion();
            }

            selectedTileIndex = -1;
        }
    }

    /// <summary>
    /// 交换层级
    /// </summary>
    /// <param name="index1"></param>
    /// <param name="index2"></param>
    void SwapTiles(int index1, int index2)
    {
        // 获取当前的sibling index
        int siblingIndex1 = tiles[index1].transform.GetSiblingIndex();
        int siblingIndex2 = tiles[index2].transform.GetSiblingIndex();

        // 交换sibling index
        tiles[index1].transform.SetSiblingIndex(siblingIndex2);
        tiles[index2].transform.SetSiblingIndex(siblingIndex1);

    }

    /// <summary>
    /// 检查每个格子的sibling index是否等于它的正确ID
    /// </summary>
    void CheckCompletion()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].transform.GetSiblingIndex() != correctOrder[i])
            {
                return; // 发现位置不对的格子
            }
        }

        // 所有格子都在正确位置
        isGameComplete = true;
        Debug.Log("恭喜！拼图完成！");

        UIManager.Instance.OpenTipPlane(112);

        // 打开
        Invoke("OpenSetParameters", 2f);
        


        // 可选：完成后的效果
        foreach (var tile in tiles)
        {
            tile.SetCompleted();
        }
    }

    /// <summary>
    /// 开始 拼图
    /// </summary>
    public void StartPlayPuzzle()
    {
        UIManager.Instance.OtherParameters(false);
        ShufflePuzzle();
        transform.GetChild(0).gameObject.SetActive(true);
    }

    /// <summary>
    /// 打开设置参数 摄像机、可以点击
    /// </summary>
    public void OpenSetParameters()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        UIManager.Instance.OtherParameters(true);

        // 保存找到的猫猫 并刷新UI
        catID_112.OnTapped();
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    public void CloseNinePuzzle()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        UIManager.Instance.OtherParameters(true);
    }
  
}
