using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

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
