using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private PuzzleManager puzzleGame;          // 拼图
    [SerializeField]
    private CameraTouchDrag cameraTouch;

    [Header("UI元素")]
    public TMP_Text foundCountText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void UpdateProgressUI()
    {
        foundCountText.text = $"{GameManager.Instance.progressManager.FoundCatCount}/{GameManager.Instance.progressManager.TotalCatCount}";
    }

    // ID_87 开始拼图
    public void StartPuzzle()
    {
        puzzleGame.OpenPuzzle();
    }
    

    // ID_77_78 网球


    public void ShowCatFoundPopup(CatBase cat)
    {
        //foundCatImage.sprite = cat.foundSprite;
        //foundCatName.text = $"猫猫 #{cat.catID}";
        //foundPopup.SetActive(true);

        //// 3秒后自动关闭
        //StartCoroutine(HidePopupAfterDelay(3f));
        Debug.Log($"猫猫 #{cat.catID} 被找到");
    }

    private IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //foundPopup.SetActive(false);
    }

    public void OnCollectibleButtonClick()
    {
        // 打开收集品图鉴
        // 实现图鉴逻辑
    }
}
