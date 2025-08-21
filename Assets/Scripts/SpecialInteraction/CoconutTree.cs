using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutTree : MonoBehaviour
{
    public InteractiveCat interactiveCat;

    private void Start()
    {
        bool isCompleted = GameManager.Instance.progressManager.IsCatFound(4);
        if (isCompleted)
        {
            this.transform.GetComponent<BoxCollider2D>().enabled = false;
            interactiveCat.PlayAnim(0, "Sports", false);
        }
    }

    public void ClickCoconut()
    { 
        this.transform.GetComponent<BoxCollider2D>().enabled = false;
        interactiveCat.PlayAnim(0, "Sports", false);
        interactiveCat.OnObjectInteracted();
    }
}
