using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance;

    [Header("七彩灯光")]

    public List<LightController> lights;

    public Color[] correctColors;
    

    public static bool isPuzzleSolved = false;

    public List<SkeletonAnimation> catList;

    public SkeletonAnimation celebrationAnim;
    public GameObject light_Sprite;

    public List<SkeletonAnimation> stageCat;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("StageKey"))
            InitializeLight();
        else
            PuzzleSolved();

        JuageSingsOpen();
        JuageStageCatUnlck();
    }

    void Update()
    {
        if (!isPuzzleSolved && CheckSolution())
        {
            PuzzleSolved();
        }
    }

    public void JuageSingsOpen()
    {
        bool isCompleted_89 = GameManager.Instance.progressManager.IsCatFound(89);
        bool isCompleted_88 = GameManager.Instance.progressManager.IsCatFound(88);
        bool isCompleted_90 = GameManager.Instance.progressManager.IsCatFound(90);
        bool isCompleted_91 = GameManager.Instance.progressManager.IsCatFound(91);

        if (isCompleted_89 && isCompleted_88 && isCompleted_90 && isCompleted_91)
            OpenStageCat();

    }

    /// <summary>
    /// 判断舞池猫猫解锁
    /// </summary>
    public void JuageStageCatUnlck()
    {
        bool isCompleted_92 = GameManager.Instance.progressManager.IsCatFound(92);
        bool isCompleted_93 = GameManager.Instance.progressManager.IsCatFound(93);

        if (isCompleted_92 && isCompleted_93)
            SwitchStageCatAnim();
    }



    /// <summary>
    /// 打开舞台猫猫
    /// </summary>
    public void OpenStageCat()
    {
        for (int i = 0; i < stageCat.Count; i++)
        {
            stageCat[i].GetComponent<MeshRenderer>().enabled = true;
            stageCat[i].enabled = true;
            stageCat[i].GetComponent<Collider2D>().enabled = true;
            stageCat[i].state.SetAnimation(0, "Stay", true);
        }
    }

    /// <summary>
    /// 切换舞台 猫猫 动画
    /// </summary>
    public void SwitchStageCatAnim()
    {
        for (int i = 0; i < stageCat.Count; i++)
        {
            stageCat[i].state.SetAnimation(0, "Sports", true);
        }

        PlayerPrefs.SetString("SwitchBGKey", "SwitchBG");
        MusicManager.Instance.PlayBGM(1);
    }


    #region 七彩灯光事件

    // 初始化灯光状态
    public void InitializeLight()
    {
        isPuzzleSolved = false;

        // 随机设置一些灯的初始状态（包括干扰色）
        foreach (LightController light in lights)
        {
            // 30%概率设置随机颜色，70%概率保持黑色
            if (Random.Range(0f, 1f) < 0.3f)
            {
                light.SetRandomColor();
            }
            else
            {
                light.SetColor(Color.black);
            }
        }
    }

    // 检查灯光是否匹配
    private bool CheckSolution()
    {
        for (int i = 0; i < lights.Count; i++)
        {
            if (lights[i].currentColor != correctColors[i])
            {
                return false;
            }
        }
        return true;
    }

    // 解锁成功
    public void PuzzleSolved()
    {
        isPuzzleSolved = true;

        // 打开所以猫猫
        OpenCat();

        // 所有灯闪烁庆祝 播放 spine 动画
        PlayCelebrationAnim();

        PlayerPrefs.SetString("StageKey", "Stageunlock");
        
    }

    
    // 打开猫猫
    public void OpenCat()
    {


        foreach (var item in catList)
        {
            item.GetComponent<MeshRenderer>().enabled = true;
            item.enabled = true;
            item.GetComponent<Collider2D>().enabled = true;
        }

    }

    // 播放庆祝动画
    public void PlayCelebrationAnim()
    {
        light_Sprite.SetActive(false);
        celebrationAnim.gameObject.SetActive(true);
        celebrationAnim.state.SetAnimation(0, "Sports", true);
    }

    #endregion








}
