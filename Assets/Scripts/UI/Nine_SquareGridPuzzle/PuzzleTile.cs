using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleTile : MonoBehaviour
{
    private Image tileImage;
    public int tileId;
    private NineSquareGridPuzzle gameController;

    private void Awake()
    {
        tileImage = GetComponent<Image>();
        this.transform.GetComponent<Button>().onClick.AddListener(OnPointerClick);
    }

    public void Initialize(int id, NineSquareGridPuzzle controller)
    {
        tileId = id;
        gameController = controller;

        // 设置初始外观
        //ResetAppearance();
    }

    public void OnPointerClick()
    {
        gameController.OnTileClicked(tileId);
    }

    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        {
            transform.localScale = Vector3.one * 0.95f;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    /// <summary>
    /// 完成后格子状态
    /// </summary>
    public void SetCompleted()
    {
        // 完成后的效果
        tileImage.color = Color.green;
    }

    public void ResetAppearance()
    {
        transform.localScale = Vector3.one;
    }
}
