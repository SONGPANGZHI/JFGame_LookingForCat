using UnityEngine;

public class SpriteItem : MonoBehaviour
{
    public string defaultItemName;
    [HideInInspector]
    public int itemLayerID;
    public Vector3 itemPos;
    public SkinsItem skinItem;
    public InteractionState interactionState;
    public int spriteID;
    public bool isClicked = false;      //是否是可点击


    public void InitData(SkinsItem _skinItem)
    {
        skinItem = _skinItem;
        defaultItemName = _skinItem.skinName;

        if (skinItem == null)
            skinItem = _skinItem;

        //1.加载图片资源
        Sprite itemSprite = Resources.Load(skinItem.imagePath + "/" + defaultItemName, typeof(Sprite)) as Sprite;
        Debug.LogError(skinItem.imagePath + "/" + defaultItemName);
        SetFurniture(itemSprite);

        //2.设置位置 
        itemPos = skinItem.localPostion;
        Debug.Log(defaultItemName + "   :itemPos:  " + itemPos.x + "     " + itemPos.y);
        transform.localPosition = itemPos;

        //3.设置层级
        itemLayerID = skinItem.orderLayer;
        this.GetComponent<SpriteRenderer>().sortingOrder = skinItem.orderLayer;

    }

    public void SetFurniture(Sprite _sprite)
    {
        if (transform.GetComponent<SpriteRenderer>().sprite == null)
        {
            transform.GetComponent<SpriteRenderer>().sprite = _sprite;
        }
    }

    //得到Sprite位置
    public void GetSpritePos()
    {
        itemPos = transform.position;
        itemLayerID = transform.GetComponent<SpriteRenderer>().sortingOrder;
        interactionState = GetComponent<SpriteItem>().interactionState;
        spriteID = GetComponent<SpriteItem>().spriteID;
    }


}
