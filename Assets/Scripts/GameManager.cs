using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("子系统")]
    public CatDatabase catDatabase;
    public ProgressManager progressManager;
    public InputManager inputManager;
    public UIManager uiManager;
    public ConditionChecker conditionChecker;
    public SaveSystem saveSystem;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if(!PlayerPrefs.HasKey(SaveSystem.SAVE_KEY))
            saveSystem.SaveGame();

        Initialize();
    }

    private void Start()
    {
        //Initialize();
    }

    private void Initialize()
    {
        catDatabase.Initialize();
        progressManager.Initialize();
        inputManager.Initialize();
        saveSystem.LoadGame();
    }

    // 游戏暂停保存
    private void OnApplicationPause(bool pause)
    {
        if (pause) saveSystem.SaveGame();
    }

    // 游戏退出保存
    private void OnApplicationQuit()
    {
        saveSystem.SaveGame();
    }

    private void Update()
    {
        // 检测是否按下Delete键
        if (Input.GetKeyDown(KeyCode.Delete))
        { 
            PlayerPrefs.DeleteAll();
            Debug.Log("已删除所有存档数据");
        }

    }
}
