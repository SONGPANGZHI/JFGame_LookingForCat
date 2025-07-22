using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleCat : CatBase
{
    private void Start()
    {
        Initialize();
    }

    // 由InputManager检测触摸
    public void OnTapped()
    {
        OnCatFound();
    }
}

