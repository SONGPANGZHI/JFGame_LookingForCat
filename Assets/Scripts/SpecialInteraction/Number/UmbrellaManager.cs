using Spine.Unity;
using UnityEngine;

/// <summary>
/// ID_96 伞
/// </summary>
public class UmbrellaManager : MonoBehaviour
{
    [Header("ID_096")]
    public int totalUmbrellas;                  // 总雨伞数量
    private int openedUmbrellas;                // 已打开的雨伞数量
    public VisibleCat hiddenCat;                // 显示猫猫
    public Transform targetPos;                 // 目标位置
    public bool isCatVisible = false;           // 是否显示猫猫

    public UniversalMovementController universalMovementController;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("UmbrellaKey"))
            openedUmbrellas = 0;
        else
            openedUmbrellas = PlayerPrefs.GetInt("UmbrellaKey");

        bool isCompleted = GameManager.Instance.progressManager.IsCatFound(96);

        if (isCompleted)
        {
            hiddenCat.transform.position = targetPos.position;
            hiddenCat.GetComponent<MeshRenderer>().enabled = true;
            hiddenCat.PlayAnim(0, "Sports", true);
            hiddenCat.catAnim.skeleton.SetColor(Color.gray);
        }

        CheckAllUmbrellasOpened();

    }

    // 报告雨伞被打开
    public void ReportUmbrellaOpened()
    {
        openedUmbrellas += 1;
        PlayerPrefs.SetInt("UmbrellaKey", openedUmbrellas);
        CheckAllUmbrellasOpened();
    }

    // 检查是否所有雨伞都已打开
    private void CheckAllUmbrellasOpened()
    {
        if (openedUmbrellas >= totalUmbrellas)
        {
            hiddenCat.GetComponent<MeshRenderer>().enabled = true;
            universalMovementController.StartMove(hiddenCat.transform, targetPos, () => 
            {
                hiddenCat.GetComponent<Collider2D>().enabled = true;
                hiddenCat.PlayAnim(0, "Sports", true);
            });

        }
    }

}
