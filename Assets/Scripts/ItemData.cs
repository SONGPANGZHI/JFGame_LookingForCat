using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Visible Objects/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemID;           // ��ƷΨһ��ʶ
    public string displayName;      // ��ʾ����
    public ItemState itemState;
    public GameObject prefab;       // ��ƷԤ���壨��ѡ��
    public bool isFound;            // �Ƿ����ҵ� - ���ֵ�ᱻ����

    [Header("��")]
    public GameObject hiddenObjects;

}
