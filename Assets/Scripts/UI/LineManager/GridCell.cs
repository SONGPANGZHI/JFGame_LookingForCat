using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridCell: MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public int x;
    public int y;
    public ColorType colorType = ColorType.White;
    public Color cellColor = Color.white;
    public bool isOccupied = false;
    public bool isPath = false;
    public bool isStartPoint = false;
    public bool isEndPoint = false;

    public Image icon;


    private Image image;
    private GridManager gridManager;

    void Awake()
    {
        image = GetComponent<Image>();
        gridManager = FindObjectOfType<GridManager>();
        UpdateVisual();
    }

    public void Initialize(int gridX, int gridY)
    {
        x = gridX;
        y = gridY;
        name = $"Cell_{x}_{y}";
    }

    public void InitSprite(Sprite _sprite,string _colorName)
    {
        icon.gameObject.SetActive(true);
        icon.sprite = _sprite;
        StringToColorType(_colorName);

        SetColorType(colorType);
    }

    private ColorType StringToColorType(string colorString)
    {
        switch (colorString)
        {
            case "CowCat": return colorType = ColorType.Red;
            case "Cat": return colorType = ColorType.Green;
            case "Ball": return colorType = ColorType.Blue;

            default: return colorType = ColorType.White;
        }


    }


    public void SetColorType(ColorType type)
    {
        colorType = type;
        cellColor = GetColorFromType(type);
        isOccupied = (type != ColorType.White); // 红色作为默认空白颜色
        UpdateVisual();
    }

    public void SetAsPath(ColorType pathColorType, bool isStart = false, bool isEnd = false)
    {
        isPath = true;
        colorType = pathColorType;
        cellColor = GetColorFromType(pathColorType);
        isOccupied = true;
        isStartPoint = isStart;
        isEndPoint = isEnd;
        UpdateVisual();
    }

    public void ResetCell(bool keepOriginalColor = false)
    {
        if (!keepOriginalColor)
        {
            colorType = ColorType.White; // 重置为红色（默认空白）
            cellColor = GetColorFromType(ColorType.White);
            isOccupied = false;
        }
        icon.gameObject.SetActive(false);
        icon.sprite = null;
        isPath = false;
        isStartPoint = false;
        isEndPoint = false;
        UpdateVisual();
    }

    private Color GetColorFromType(ColorType type)
    {
        switch (type)
        {
            case ColorType.Red: return Color.red;
            case ColorType.Green: return Color.green;
            case ColorType.Blue: return Color.blue;
            case ColorType.White: return Color.white;
            default: return Color.white;
        }
    }

    private void UpdateVisual()
    {
        if (image != null)
        {
            image.color = cellColor;

            if (isStartPoint || isEndPoint)
            {
                image.transform.localScale = Vector3.one * 1.1f;
                // 为起点和终点添加边框效果
                image.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
            }
            else
            {
                image.transform.localScale = Vector3.one;
                image.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gridManager != null && gridManager.IsDrawing)
        {
            gridManager.HandleCellHover(this);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gridManager != null)
        {
            gridManager.HandleCellClick(this);
        }
    }
}
