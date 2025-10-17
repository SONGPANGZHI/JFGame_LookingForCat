using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlidingPuzzleTile : MonoBehaviour
{
    [HideInInspector]
    public Vector2Int currentPos;
    [HideInInspector]
    public int spriteIndex; // 这个拼图块对应的图片索引

    private Image tileImage;
    private SlidingPuzzleManager puzzleManager;

    void Awake()
    {
        tileImage = GetComponent<Image>();
        puzzleManager = FindObjectOfType<SlidingPuzzleManager>();
    }

    public void SetTile(Sprite sprite, Vector2Int position, int index)
    {
        tileImage.sprite = sprite;
        currentPos = position;
        spriteIndex = index;
    }

    public void UpdatePosition(Vector2Int newPosition)
    {
        currentPos = newPosition;
    }

    public void OnPointerClick()
    {
        puzzleManager.OnTileClicked(currentPos);
    }
}
