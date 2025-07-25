using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPuzzle : MonoBehaviour
{
    //点击切换图片
    public List<Sprite> puzzleImages; // 存储所有的拼图图片
    public SpriteRenderer spriteRenderer; // 用于显示当前拼图图片
    private int currentImageIndex = 0; // 当前显示的图片索引
    void Start()
    {
        if (puzzleImages.Count > 0)
        {
            spriteRenderer.sprite = puzzleImages[currentImageIndex]; // 初始化显示第一张图片
        }
    }

    //点击切换图片
    public void OnPointClick()
    {
        SwitchImage(); // 切换到下一张图
    }

    // 切换到下一张图片
    void SwitchImage()
    {
        currentImageIndex = (currentImageIndex + 1) % puzzleImages.Count; // 循环切换图片索引
        spriteRenderer.sprite = puzzleImages[currentImageIndex]; // 更新显示的图片
    }
}
