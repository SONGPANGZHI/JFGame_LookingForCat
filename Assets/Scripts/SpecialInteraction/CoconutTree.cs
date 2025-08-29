using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ID_004 猫猫
/// </summary>
public class CoconutTree : MonoBehaviour
{
    public InteractiveCat interactiveCat_004;

    public VisibleCat visibleCat_026;


    private void Start()
    {
        bool isCompleted = GameManager.Instance.progressManager.IsCatFound(4);
        if (isCompleted)
        {
            this.transform.GetComponent<BoxCollider2D>().enabled = false;
            interactiveCat_004.PlayAnim(0, "Sports", false);
        }
    }

    /// <summary>
    /// 点击 ID_004 猫猫交互
    /// </summary>
    public void ClickCoconut()
    { 
        this.transform.GetComponent<BoxCollider2D>().enabled = false;
        interactiveCat_004.PlayAnim(0, "Sports", false);
        Invoke("ShowClick",1f);
    }

    public void ShowClick()
    {
        interactiveCat_004.OnObjectInteracted();
    }

    /// <summary>
    /// 点击 ID_026 椰子树叶
    /// </summary>
    public void ClickCoconutLeaves()
    { 
    
    }
}
