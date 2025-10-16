using UnityEngine;
using UnityEngine.EventSystems;


    public enum BlockType
    {
        Horizontal,
        Vertical,
        Vertical_2,
        Target
    }

    public class KlotskiBlock : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
    public BlockType type;
    public Vector2Int[] positions;
    public int instanceID;

    private Vector2 dragStartPosition;
    private Vector2 blockStartPosition;
    private bool isDragging = false;
    private Vector2Int lastMoveDirection;

    public void Initialize(BlockType blockType, Vector2Int[] blockPositions, int id)
    {
        type = blockType;
        positions = blockPositions;
        instanceID = id;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (KlotskiManager.Instance.isGameOver) return;

        dragStartPosition = eventData.position;
        blockStartPosition = transform.localPosition;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging || KlotskiManager.Instance.isGameOver) return;

        Vector2 dragDelta = eventData.position - dragStartPosition;
        Vector2Int moveDirection = GetMoveDirection(dragDelta);

        if (moveDirection != Vector2Int.zero && moveDirection != lastMoveDirection)
        {
            TryMove(moveDirection);
            lastMoveDirection = moveDirection;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        lastMoveDirection = Vector2Int.zero;
    }

    /// <summary>
    /// 移动获得新位置
    /// </summary>
    /// <param name="dragDelta"></param>
    /// <returns></returns>
    Vector2Int GetMoveDirection(Vector2 dragDelta)
    {
        float minDragDistance = 30F;

        if (Mathf.Abs(dragDelta.x) > Mathf.Abs(dragDelta.y))
        {
            if (Mathf.Abs(dragDelta.x) > minDragDistance)
            {
              
                if (type == BlockType.Horizontal || type == BlockType.Target)
                {
                    return new Vector2Int((int)Mathf.Sign(dragDelta.x), 0);
                }
            }
        }
        else
        {
            if (Mathf.Abs(dragDelta.y) > minDragDistance)
            {
                if (type == BlockType.Vertical|| type == BlockType.Vertical_2)
                {
                    return new Vector2Int(0, (int)Mathf.Sign(-dragDelta.y));
                }
            }
        }

        return Vector2Int.zero;
    }


    void TryMove(Vector2Int direction)
    {
        if (KlotskiManager.Instance.CanMoveBlock(this, direction))
        {
            KlotskiManager.Instance.MoveBlock(this, direction);
        }
    }

}

