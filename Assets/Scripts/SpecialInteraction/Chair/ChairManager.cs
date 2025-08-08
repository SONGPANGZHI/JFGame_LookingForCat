using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//拼图管理
public class ChairManager : MonoBehaviour
{
    public ChairPiece[] allPieces;
    public int assembledCount = 0;

    public void PieceAssembled()
    {
        assembledCount++;
        if (assembledCount >= allPieces.Length)
        {
            Debug.Log("所有零件组装完成！");
            // 这里可以触发完成事件，如播放动画、显示UI等
        }
    }

}
