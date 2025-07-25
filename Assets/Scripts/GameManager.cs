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
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        catDatabase.Initialize();
        progressManager.Initialize();
        inputManager.Initialize();
        saveSystem.LoadGame();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) saveSystem.SaveGame();
    }

    private void OnApplicationQuit()
    {
        saveSystem.SaveGame();
    }
}
