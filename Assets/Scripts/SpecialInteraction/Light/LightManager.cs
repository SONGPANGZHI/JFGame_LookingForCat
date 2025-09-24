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
    }

    void Update()
    {
        if (!isPuzzleSolved && CheckSolution())
        {
            PuzzleSolved();
        }
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
