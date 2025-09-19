using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour
{
    public bool isPlaced = false;

    public void Move(Vector2 direction)
    {
        if (!isPlaced)
        {
            transform.Translate(direction);
        }
    }

    public void Rotate(float angle)
    {
        if (!isPlaced)
        {
            transform.Rotate(0, 0, angle);
        }
    }

    public void SetActive(bool active)
    {
        // 可以在这里添加高亮效果
        if (active)
        {
            GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        else
        {
            GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        }
    }
}
