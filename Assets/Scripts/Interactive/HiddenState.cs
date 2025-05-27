using UnityEngine;

/// <summary>
/// ����״̬��HiddenState���ڻ���״̬�У���Ʒ����ֱ�ӿɼ��ģ�����������������Ʒ�£�������Ҷ�����Ӵ򿪣���
/// </summary>
public class HiddenState : InteractionState
{
    private ItemClick hiddenItem;  // �����ص���Ʒ
    private GameObject interactableObject;  // �ɻ��������壨������Ҷ�����ӵȣ�

    public HiddenState(ItemClick item, GameObject interactable)
    {
        hiddenItem = item;
        interactableObject = interactable;
    }

    public override void EnterState()
    {
        Debug.Log("���뻥��״̬");
        // �������û�������Ϊ�ɵ����������ʾ������ʾ
        interactableObject.SetActive(true);
    }

    public override void ExitState()
    {
        Debug.Log("�˳�����״̬");
        // �����������״̬
        interactableObject.SetActive(false);
    }

    public override void UpdateState()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == interactableObject)
                {
                    interactableObject.SetActive(false);  // �Ƴ��������壬��ʾ������Ʒ
                    hiddenItem.gameObject.SetActive(true);  // ��ʾ���ص���Ʒ
                    hiddenItem.Found();  // �����ƷΪ���ҵ�
                }
            }
        }
    }
}
