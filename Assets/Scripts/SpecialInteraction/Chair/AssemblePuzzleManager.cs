using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//组装拼图管理
public class AssemblePuzzleManager : MonoBehaviour
{
    public static AssemblePuzzleManager Instance { get; private set; }

    [Header("拼图集合")]
    [SerializeField] 
    private List<PuzzleSet> puzzleSets = new List<PuzzleSet>();

    private Dictionary<int, PuzzleSet> puzzleDict;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        // 初始化拼图字典
        InitializeDictionary();
    }

    private void Start()
    {
        CheckAllPuzzlesCompletion();
    }

    /// <summary>
    /// 检查所有拼图完成状态（从存档加载时调用）
    /// </summary>
    public void CheckAllPuzzlesCompletion()
    {
        foreach (var set in puzzleSets)
        {
            bool isCompleted = GameManager.Instance.progressManager.IsCatFound(set.puzzleID);
            set.isCompleted = isCompleted;
            if (isCompleted)
                TriggerPuzzleCompletion(set.puzzleID);
        }
    }

    /// <summary>
    /// 直接触发拼图完成状态（用于读档时恢复）
    /// </summary>
    private void TriggerPuzzleCompletion(int puzzleID)
    {
        if (!puzzleDict.TryGetValue(puzzleID, out PuzzleSet set)) return;

        // 强制所有部件进入完成状态
        foreach (var piece in set.pieces)
        {
            piece.OnPuzzlePieceCompleted();
        }

        foreach (var cat in set.catOBJ)
        {
            cat.GetComponent<SpriteRenderer>().enabled = true;
            cat.GetComponent<Collider2D>().enabled = true;
        }
    }

    /// <summary>
    /// 初始化拼图字典
    /// </summary>
    private void InitializeDictionary()
    {
        puzzleDict = new Dictionary<int, PuzzleSet>();
        foreach (var set in puzzleSets)
        {
            if (!puzzleDict.ContainsKey(set.puzzleID))
            {
                puzzleDict.Add(set.puzzleID, set);
            }
            else
            {
                Debug.LogWarning($"重复的拼图ID: {set.puzzleID}");
            }
        }
    }

    /// <summary>
    /// 部件组装完成
    /// </summary>
    /// <param name="puzzleID"></param>
    public void  PieceAssembled(int puzzleID)
    {
        if (puzzleDict.TryGetValue(puzzleID, out PuzzleSet set))
        {
            set.assembledCount += 1; 

            if (set.assembledCount >= set.pieces.Length)
            {
                Debug.Log($"拼图 {puzzleID} 所有零件组装完成！");

                // 触发完成事件
                set.isCompleted = true;
                
                foreach (var cat in set.catOBJ)
                {
                    cat.GetComponent<SpriteRenderer>().enabled = true;
                    cat.GetComponent<Collider2D>().enabled = true;
                }

            }
            else
            {
                Debug.LogWarning($"未知的拼图ID: {puzzleID}");
            }
        }

    }

   
}
