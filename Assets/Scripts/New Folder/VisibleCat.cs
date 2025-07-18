using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleCat : CatBase
{
    private void Start()
    {
        Initialize();
    }

    // ÓÉInputManager¼ì²â´¥Ãþ
    public void OnTapped()
    {
        OnCatFound();
    }
}
